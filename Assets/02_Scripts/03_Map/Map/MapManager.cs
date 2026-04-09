using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public enum NodeType { Monster = 0, Elite, Store, Shelter, Event, Boss, None }

[System.Serializable]
public class MapNode
{
    public int x, y; 
    public NodeType nodeType = NodeType.None; 
    public bool isActive = false; 
    public bool hasBattlePreset = false;
    public MonsterLevel battleLevel = MonsterLevel.Normal;
    public int minMonsterScore = 0;
    public int maxMonsterScore = 0;

    [System.NonSerialized]
    public List<MapNode> nextNodes = new List<MapNode>(3);
}
public class Branch
{
    // Dictionary에서 Enum을 인덱스로 쓰는 배열 캐싱
    private readonly NodeTypeDataEntry[] fastRules;
    
    private int[] lastSeenStep; // 특정 타입이 마지막으로 등장한 층 기록
    private int[] spawnCount;   // 특정 타입이 지금까지 몇 번 등장했는지 기록
    private int typeCount;      // 전체 노드 타입의 개수

    public Branch(Dictionary<NodeType, NodeTypeDataEntry> rules)
    {
        this.typeCount = System.Enum.GetValues(typeof(NodeType)).Length;
        this.fastRules = new NodeTypeDataEntry[typeCount];
        
        foreach (var kvp in rules)
        {
            fastRules[(int)kvp.Key] = kvp.Value;
        }

        this.lastSeenStep = new int[typeCount];
        this.spawnCount = new int[typeCount];
        
        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < typeCount; i++)
        {
            lastSeenStep[i] = -10; // 초기값을 음수로 주어 첫 등장 시 쿨타임에 걸리지 않게 함
            spawnCount[i] = 0;
        }
    }

    // 후반부 확률로 교체할때 전반부 기록 이어받는용 함수
    public void InheritData(Branch oldBranch)
    {
        System.Array.Copy(oldBranch.lastSeenStep, this.lastSeenStep, typeCount);
        System.Array.Copy(oldBranch.spawnCount, this.spawnCount, typeCount);
    }

    /// 현재 층에서 등장할 수 있는 조건 검사
    public bool CanSpawn(NodeType type, int currentFloor)
    {
        int idx = (int)type; 
        NodeTypeDataEntry rule = fastRules[idx];
        
        if (rule == null) return false;
        
        if (rule.maxSpawnCount != -1 && spawnCount[idx] >= rule.maxSpawnCount) return false; // 최대 등장 횟수 초과
        if (currentFloor < rule.minSpawnStep) return false; // 아직 나올 수 있는 층이 아님
        if (currentFloor - lastSeenStep[idx] <= rule.distanceMin) return false; // 연속 등장 방지

        return true;
    }

    // 천장 시스템 검사
    public bool CheckPity(NodeType type, int currentFloor)
    {
        int idx = (int)type; 
        NodeTypeDataEntry rule = fastRules[idx];
        
        if (rule == null) return false;
        if (rule.maxSpawnCount != -1 && spawnCount[idx] >= rule.maxSpawnCount) return false;
        if (currentFloor < rule.minSpawnStep) return false;
        if (rule.distanceMax <= 0) return false; // 천장 설정이 없으면 패스

        int lastSeen = (lastSeenStep[idx] < 0) ? 0 : lastSeenStep[idx];
        return (currentFloor - lastSeen >= rule.distanceMax); // 천장 도달 여부
    }

    public void OnNodeSelected(NodeType type, int currentFloor)
    {
        int idx = (int)type;
        lastSeenStep[idx] = currentFloor; 
        spawnCount[idx]++;                
    }

    /// 가로줄 잭팟(상점 3개 등)이 터져서 노드를 강제로 바꿨을 때, 과거 기록을 롤백하는 함수
    
    public void ModifyHistory(NodeType oldType, NodeType newType, int currentFloor)
    {
        int oldIdx = (int)oldType;
        if (spawnCount[oldIdx] > 0) spawnCount[oldIdx]--; // 취소된 타입의 카운트 원상복구
        OnNodeSelected(newType, currentFloor);
    }
}

public class MapManager : Singleton<MapManager>
{
    // 데모 버전 임시 고정 스테이지 순서 (1층 -> 8층)
    private static readonly NodeType[] DemoStageSequence =
    {
        NodeType.Monster,
        NodeType.Event,
        NodeType.Monster,
        NodeType.Store,
        NodeType.Shelter,
        NodeType.Elite,
        NodeType.Shelter,
        NodeType.Boss
    };

    [Header("PlayerSettings")]
    public PlayerMove playerMove;

    [Header("Map Settings")]
    // [SerializeField] private int width = 3;   // 기존 3열 설정 (데모 동안 비활성화)
    [SerializeField] private int width = 1;   // 데모용 고정 1열
    [SerializeField] private int height = 8; // 데모용 고정 8층

    [Header("Stage Data")]
    [SerializeField] private NodeTypeData earlyStageData; // 1~10층 확률 데이터 
    [SerializeField] private NodeTypeData lateStageData;  // 11~20층 확률 데이터

    // 맵 전체 데이터를 담고 있는 핵심 2차원 리스트 [층][열]
    public List<List<MapNode>> mapGrid;
    
    // 가로줄(width) 개수만큼 할당되는 확률 추적기 리스트
    private List<Branch> branches;
    
    private Dictionary<NodeType, NodeTypeDataEntry> earlyRules;
    private Dictionary<NodeType, NodeTypeDataEntry> lateRules;
    
    // 플레이어가 현재 서 있는 위치
    private MapNode currentNode = null;
    private bool isNodeChange = false;
    public MapNode CurrentNode => currentNode;
    private bool hasEncounterPreset;
    private int encounterScore;
    private MonsterLevel encounterLevel = MonsterLevel.Normal;

    // 매 프레임 new List()를 막기 위해 멤버 변수로 미리 할당한 천장용 임시 리스트
    private readonly List<NodeType> cachedPityList = new List<NodeType>(10);

    protected override void Awake()
    {
        base.Awake();

        ResolveReferences();
        InitRuleData(); 
        InitBranches();
        GenerateMap();

        if (playerMove != null)
        {
            playerMove.gameObject.SetActive(false); // 맵이 다 그려지기 전까지 플레이어 숨김
        }
    } 

    private void ResolveReferences()
    {
        if (playerMove == null)
        {
            playerMove = FindFirstObjectByType<PlayerMove>(FindObjectsInactive.Include);
        }
    }
    
    private void InitRuleData()
    {
        earlyRules = earlyStageData != null ? earlyStageData.GetNodeMap() : new Dictionary<NodeType, NodeTypeDataEntry>();
        lateRules = lateStageData != null ? lateStageData.GetNodeMap() : new Dictionary<NodeType, NodeTypeDataEntry>();
    }

    private void InitBranches()
    {
        branches = new List<Branch>(width);
        for (int i = 0; i < width; i++)
        {
            branches.Add(new Branch(earlyRules)); // 초기에는 전반부 룰 세팅
        }
    }

    // 맵 결정 메인로직
    public void GenerateMap()
    {
        // 데모 버전은 1열 고정
        width = 1;

        // 데모 버전은 요청된 8스테이지 고정 순서를 항상 사용
        height = DemoStageSequence.Length;

        // 1. 빈 맵 그리드 생성
        mapGrid = new List<List<MapNode>>(height);
        for (int y = 0; y < height; y++)
        {
            List<MapNode> row = new List<MapNode>(width);
            for (int x = 0; x < width; x++) row.Add(new MapNode { x = x, y = y });
            mapGrid.Add(row);
        }

        // 2. 데모 고정 맵 생성
        int centerIndex = width / 2;
        for (int floor = 1; floor <= height; floor++)
        {
            int floorIndex = floor - 1;
            NodeType fixedType = DemoStageSequence[floorIndex];
            bool isSingleNodeFloor = (fixedType == NodeType.Shelter || fixedType == NodeType.Boss);

            for (int x = 0; x < width; x++)
            {
                MapNode node = mapGrid[floorIndex][x];

                if (isSingleNodeFloor && x != centerIndex)
                {
                    node.nodeType = NodeType.None;
                    node.isActive = false;
                    node.hasBattlePreset = false;
                    node.minMonsterScore = 0;
                    node.maxMonsterScore = 0;
                    continue;
                }

                node.nodeType = fixedType;
                node.isActive = true;
                ApplyDemoBattlePreset(node, floor);
            }
        }

        /*
        [기존 랜덤 생성 로직 - 데모 버전 동안 임시 비활성화]

        List<NodeType> floorTypes = new List<NodeType>(width);
        Dictionary<NodeType, NodeTypeDataEntry> currentRules = earlyRules;

        for (int floor = 1; floor <= height; floor++)
        {
             int floorIndex = floor - 1;

            if (floor == 11)
            {
                currentRules = lateRules;
                List<Branch> oldBranches = new List<Branch>(branches);
                branches.Clear();

                for (int i = 0; i < width; i++)
                {
                    Branch newBranch = new Branch(lateRules);
                    newBranch.InheritData(oldBranches[i]);
                    branches.Add(newBranch);
                }
            }

            floorTypes.Clear();

            bool isSingleNodeFloor = (floor == 10 || floor == 19 || floor == 20);
            int center = width / 2;

            for (int x = 0; x < width; x++)
            {
                MapNode node = mapGrid[floorIndex][x];

                if (isSingleNodeFloor && x != center)
                {
                    node.nodeType = NodeType.None;
                    node.isActive = false;
                    continue;
                }

                NodeType selectedType = DetermineNodeType(branches[x], floor, currentRules);

                node.nodeType = selectedType;
                node.isActive = true;

                floorTypes.Add(selectedType);
                branches[x].OnNodeSelected(selectedType, floor);
            }

            if (!isSingleNodeFloor)
            {
                CheckAndFixJackpot(floorIndex, floor, floorTypes);
            }
        }
        */
        
        // 3. 결정된 노드들을 연결하는 선 긋기
        GenerateFixedPaths();
    }
    // 데모 버전용 노드별 고정 배틀 프리셋 설정
    private void ApplyDemoBattlePreset(MapNode node, int floor)
    {
        node.hasBattlePreset = false;
        node.battleLevel = MonsterLevel.Normal;
        node.minMonsterScore = 0;
        node.maxMonsterScore = 0;

        if (node.nodeType == NodeType.Monster)
        {
            node.hasBattlePreset = true;
            node.battleLevel = MonsterLevel.Normal;
            if (floor == 1)
            {
                node.minMonsterScore = 2;
                node.maxMonsterScore = 2;
            }
            else if (floor < 8)
            {
                node.minMonsterScore = 2;
                node.maxMonsterScore = 3;
            }
            else
            {
                node.minMonsterScore = 3;
                node.maxMonsterScore = 4;
            }
            return;
        }

        if (node.nodeType == NodeType.Elite)
        {
            node.hasBattlePreset = true;
            node.battleLevel = MonsterLevel.Elite;
            node.minMonsterScore = 5;
            node.maxMonsterScore = 5;
            return;
        }

        if (node.nodeType == NodeType.Boss)
        {
            node.hasBattlePreset = true;
            node.battleLevel = MonsterLevel.Boss;
            node.minMonsterScore = 9;
            node.maxMonsterScore = 9;
        }
    }
    
    // 노드 타입 결정 로직
    private NodeType DetermineNodeType(Branch branch, int floor, Dictionary<NodeType, NodeTypeDataEntry> rules)
    {
        if (floor == 1) return NodeType.Monster;
        if (floor == 10) return NodeType.Shelter;
        if (floor == 19) return NodeType.Shelter;
        if (floor == 20) return NodeType.Boss;


        cachedPityList.Clear(); 
        foreach (var key in rules.Keys)
        {
            if (branch.CheckPity(key, floor)) cachedPityList.Add(key);
        }
        
        // 천장에 도달한 방이 있다면 그 중 하나를 무조건 출현
        if (cachedPityList.Count > 0) return cachedPityList[Random.Range(0, cachedPityList.Count)];

        // 랜덤 뽑기
        float totalWeight = 0f;
        foreach (var entry in rules.Values)
        {
            if (branch.CanSpawn(entry.type, floor)) totalWeight += entry.ratio;
        }

        if (totalWeight <= 0) return NodeType.Monster; // 나올 게 없으면 몬스터로 기본 고정

        float randomValue = Random.Range(0, totalWeight);
        foreach (var entry in rules.Values)
        {
            if (branch.CanSpawn(entry.type, floor))
            {
                randomValue -= entry.ratio;
                if (randomValue <= 0) return entry.type;
            }
        }

        return NodeType.Monster;
    }

    // 잭팟시 기본 몬스터로 1개 강제변경
    private void CheckAndFixJackpot(int floorIndex, int currentFloor, List<NodeType> types)
    {
        if (types.Count >= 3)
        {
            NodeType first = types[0];
            if (first != NodeType.Monster) // 몬스터 3마리는 정상
            {
                bool allSame = true;
                for(int i = 1; i < types.Count; i++)
                {
                    if(types[i] != first) { allSame = false; break; }
                }

                if (allSame) // 전부 똑같다면
                {
                    int targetIdx = Random.Range(0, width);
                    mapGrid[floorIndex][targetIdx].nodeType = NodeType.Monster;
                    branches[targetIdx].ModifyHistory(first, NodeType.Monster, currentFloor); // 롤백
                }
            }
        }
    }


    // 층별 형태에 맞춰 노드연결 설정

    private void GenerateFixedPaths()
    {
        foreach (var row in mapGrid)
            foreach (var node in row)
            {
                node.nextNodes.Clear(); 
            }

        for (int y = 0; y < height - 1; y++)
        {
            List<int> nextActiveXs = new List<int>(width);
            for (int nextX = 0; nextX < width; nextX++)
            {
                if (mapGrid[y + 1][nextX].isActive) nextActiveXs.Add(nextX);
            }

            for (int x = 0; x < width; x++)
            {
                MapNode currentNode = mapGrid[y][x];
                if (!currentNode.isActive) continue;

                // 다음 층 활성 노드가 1개면 수렴, 아니면 같은 열로 직진
                if (nextActiveXs.Count == 1)
                {
                    TryConnect(currentNode, nextActiveXs[0], y + 1);
                }
                else
                {
                    TryConnect(currentNode, x, y + 1);
                }
            }
        }
    }

    private void TryConnect(MapNode fromNode, int targetX, int targetY)
    {
        if (targetY >= height || targetX < 0 || targetX >= width) return;
        
        MapNode targetNode = mapGrid[targetY][targetX];
        if (!targetNode.isActive) return;
        
        fromNode.nextNodes.Add(targetNode); // 다음 갈 수 있는 방으로 등록
    }
 

    // UI 맵 화면에서 노드(버튼)를 클릭했을 때 호출

    public void OnNodeClicked(MapNode targetNode)
    {
        OnNodeClickedAsync(targetNode).Forget();
    }

    private async UniTaskVoid OnNodeClickedAsync(MapNode targetNode)
    {
        if (playerMove == null || MapVisualizer.Instance == null || ButtonEvent.Instance == null) return;
        if (isNodeChange) return;
        if (playerMove.gameObject.activeSelf && playerMove.IsMoving) return;

        // 갈 수 있는 정상적인 길인지 검증
        if (CheckIsPathValid(targetNode))
        {
            isNodeChange = true;

            if (currentNode == null) // 게임 최초 시작 시 (1층 클릭)
            {
                playerMove.gameObject.SetActive(true);
                MapVisualizer.Instance.SetPlayerPositionDirectly(targetNode, new Vector3(0, -100f, 0));
            }

            currentNode = targetNode;

            // 이동 연출(시각 효과) 호출
            await MapVisualizer.Instance.MovePlayerAsync(targetNode);

            if (MapVisualizer.Instance == null || ButtonEvent.Instance == null)
            {
                isNodeChange = false;
                return;
            }

            // 클릭 가능한 버튼 상태 갱신
            MapVisualizer.Instance.UpdateNodeInteractivity(currentNode);

            ButtonEvent.Instance.EnterNode(targetNode);
            isNodeChange = false;
        }
    }


    /// 클릭한 대상 노드가 현재 위치에서 이어져 있는 정상적인 길인지 체크
    private bool CheckIsPathValid(MapNode targetNode)
    {
        if (currentNode == null) return targetNode.y == 0; // 시작 전엔 1층(index 0)만 가능
        return currentNode.nextNodes.Contains(targetNode); // 내 다음 노드 리스트에 있는지 확인
    }

    public void SetEncounterPreset(int score, MonsterLevel level)
    {
        hasEncounterPreset = true;
        encounterScore = score;
        encounterLevel = level;
    }


    // 데모버전 임시 정예, 보스 스테이지 몬스터 스코어 고정 프리셋 제공 함수
    public bool DemoMonsterScorePreset(out int score, out MonsterLevel level)
    {
        if (!hasEncounterPreset)
        {
            score = 0;
            level = MonsterLevel.Normal;
            return false;
        }

        score = encounterScore;
        level = encounterLevel;
        hasEncounterPreset = false;
        return true;
    }

    public int GetCurrentFloor() => currentNode == null ? 0 : currentNode.y; 
}