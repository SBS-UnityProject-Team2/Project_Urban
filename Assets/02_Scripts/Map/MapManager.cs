using System.Collections.Generic;
using UnityEngine;

// 1. 노드 타입 정의
public enum NodeType { Monster = 0, Elite, Store, Shelter, Event, Boss, None }


// 2. 맵 노드 데이터 클래스
[System.Serializable]
public class MapNode
{
    // 그리드 상의 좌표 (x: 열, y: 층)
    public int x, y;
    public NodeType nodeType = NodeType.None;
    
    // 실제 게임에서 활성화되어 갈 수 있는 노드인지 여부
    public bool isActive = false;
    
    [System.NonSerialized]
    public List<MapNode> nextNodes = new List<MapNode>(3);
}

public class Branch
{
    // 규칙 데이터
    private readonly Dictionary<NodeType, NodeTypeDataEntry> ruleBook;
    
    private int[] lastSeenStep; // 해당 타입이 마지막으로 등장한 층
    private int[] spawnCount;   // 해당 타입이 총 몇 번 등장했는지
    private int typeCount;      // 노드 타입의 총 개수

    public Branch(Dictionary<NodeType, NodeTypeDataEntry> rules)
    {
        this.ruleBook = rules;
        
        // Enum의 총 개수를 구해 배열 크기를 할당
        this.typeCount = System.Enum.GetValues(typeof(NodeType)).Length;
        this.lastSeenStep = new int[typeCount];
        this.spawnCount = new int[typeCount];
        
        ResetState();
    }

    // 상태 초기화
    private void ResetState()
    {
        for (int i = 0; i < typeCount; i++)
        {
            lastSeenStep[i] = -10; 
            spawnCount[i] = 0;
        }
    }

    public void InheritData(Branch oldBranch)
    {
        System.Array.Copy(oldBranch.lastSeenStep, this.lastSeenStep, typeCount);
    }

    // 조건 체크
    public bool CanSpawn(NodeType type, int currentFloor)
    {
        // 규칙이 없는 타입이면 생성 불가
        if (!ruleBook.TryGetValue(type, out var rule)) return false;
        
        int idx = (int)type; // Enum -> int 변환
        
        // 1. 최대 등장 횟수 초과 체크
        if (rule.maxSpawnCount != -1 && spawnCount[idx] >= rule.maxSpawnCount) return false;
        
        // 2. 최소 등장 층 체크
        if (currentFloor < rule.minSpawnStep) return false;
        
        // 3. 쿨타임(최소 간격) 체크
        if (currentFloor - lastSeenStep[idx] <= rule.distanceMin) return false;

        return true;
    }

    // 너무 오랫동안 안 나왔는가? 
    public bool CheckPity(NodeType type, int currentFloor)
    {
        if (!ruleBook.TryGetValue(type, out var rule)) return false;
        
        int idx = (int)type; 
        
        // 기본 조건(개수, 최소 층)이 안 맞으면 천장 발동 안 함
        if (rule.maxSpawnCount != -1 && spawnCount[idx] >= rule.maxSpawnCount) return false;
        if (currentFloor < rule.minSpawnStep) return false;
        
        // 천장 설정이 없는 경우 패스
        if (rule.distanceMax <= 0) return false;

        int lastSeen = (lastSeenStep[idx] < 0) ? 0 : lastSeenStep[idx];
        
        // 지정된 최대 간격 이상 안 나왔다면 true 반환
        return (currentFloor - lastSeen >= rule.distanceMax);
    }

    // 노드가 확정되었을 때 기록 업데이트
    public void OnNodeSelected(NodeType type, int currentFloor)
    {
        int idx = (int)type;
        lastSeenStep[idx] = currentFloor; // 마지막 등장 층 갱신
        spawnCount[idx]++;                // 등장 횟수 증가
    }

    // 잭팟 방지
    public void ModifyHistory(NodeType oldType, NodeType newType, int currentFloor)
    {
        int oldIdx = (int)oldType;
        
        // 취소된 타입의 카운트 감소
        if (spawnCount[oldIdx] > 0) spawnCount[oldIdx]--;
        
        OnNodeSelected(newType, currentFloor);
    }
}

// 4. 맵 매니저
public class MapManager : Singleton<MapManager>
{
    [Header("PlayerSettings")]
    [Tooltip("플레이어 이동을 담당하는 스크립트")]
    public PlayerMove playerMove;

    [Header("Map Settings")]
    [SerializeField] private int width = 3;   // 맵의 가로 폭 (라인 수)
    [SerializeField] private int height = 20; // 전체 층 수

    [Header("Stage Data")]
    [Tooltip("1~10층에 적용될 노드 확률 데이터")]
    [SerializeField] private NodeTypeData earlyStageData; 
    [Tooltip("11~20층에 적용될 노드 확률 데이터")]
    [SerializeField] private NodeTypeData lateStageData;

    // 맵 전체 구조를 저장하는 2차원 리스트
    public List<List<MapNode>> mapGrid;

    // 각 라인별 확률 관리자
    private List<Branch> branches;
    
    // ScriptableObject에서 변환된 런타임 규칙 데이터
    private Dictionary<NodeType, NodeTypeDataEntry> earlyRules;
    private Dictionary<NodeType, NodeTypeDataEntry> lateRules;
    
    // 플레이어가 현재 위치한 노드
    private MapNode currentNode = null;
    //외부에서 접근가능하도록 프로퍼티 추가
    public MapNode CurrentNode => currentNode;

    protected override void Awake()
    {
        base.Awake();

        InitRuleData(); 
        InitBranches();
        GenerateMap(); // 맵 생성 시작       
        playerMove.gameObject.SetActive(false);
    } 
    
    // ScriptableObject 데이터를 런타임에서 사용할 Dictionary로 변환
    private void InitRuleData()
    {
        if (earlyStageData != null) earlyRules = earlyStageData.GetNodeMap();
        else earlyRules = new Dictionary<NodeType, NodeTypeDataEntry>();

        if (lateStageData != null) lateRules = lateStageData.GetNodeMap();
        else lateRules = new Dictionary<NodeType, NodeTypeDataEntry>();
    }

    // 각 라인별로 Branch 객체 생성
    private void InitBranches()
    {
        // 리스트 크기를 미리 지정하여 메모리 재할당 방지
        branches = new List<Branch>(width);
        for (int i = 0; i < width; i++)
        {
            // 기본적으로 초반 규칙(earlyRules)으로 시작
            branches.Add(new Branch(earlyRules));
        }
    }

    // 맵 생성 메인 로직
    public void GenerateMap()
    {
        // 1. 그리드(MapNode) 초기화
        mapGrid = new List<List<MapNode>>(height);
        for (int y = 0; y < height; y++)
        {
            List<MapNode> row = new List<MapNode>(width);
            // 좌표값(x, y)을 가진 빈 노드 생성
            for (int x = 0; x < width; x++) row.Add(new MapNode { x = x, y = y });
            mapGrid.Add(row);
        }

        // 해당 층에서 결정된 노드 타입들을 임시 저장할 리스트 (잭팟 방지용)
        List<NodeType> floorTypes = new List<NodeType>(width);

        // 현재 적용 중인 규칙 (전반부엔 earlyRules)
        Dictionary<NodeType, NodeTypeDataEntry> currentRules = earlyRules;

        // 2. 1층부터 꼭대기까지 올라가며 노드 타입 결정
        for (int floor = 1; floor <= height; floor++)
        {
             int floorIndex = floor - 1; // 리스트 인덱스는 0부터 시작

            // 11층부터 후반부 규칙 적용
            if (floor == 11)
            {
                currentRules = lateRules;
                List<Branch> oldBranches = new List<Branch>(branches);
                branches.Clear();
                
                // 기존 Branch의 기록(쿨타임 등)을 계승하여 새 Branch 생성
                for(int i=0; i<width; i++) 
                {
                    Branch newBranch = new Branch(lateRules);
                    newBranch.InheritData(oldBranches[i]); 
                    branches.Add(newBranch);
                }
            }

            floorTypes.Clear();
            
            // 특수 층 확인 (보스방, 휴식방 등은 중앙 외길)
            bool isSingleNodeFloor = (floor == 10 || floor == 19 || floor == 20);
            int centerIndex = width / 2; 

            for (int x = 0; x < width; x++)
            {
                MapNode node = mapGrid[floorIndex][x];

                // 외길 층 처리: 중앙이 아니면 비활성화
                if (isSingleNodeFloor && x != centerIndex)
                {
                    node.nodeType = NodeType.None;
                    node.isActive = false;
                    continue;
                }

                // 해당 칸의 노드 타입 결정
                NodeType selectedType = DetermineNodeType(branches[x], floor, currentRules);
                
                // 데이터 적용
                node.nodeType = selectedType;
                node.isActive = true;
                
                floorTypes.Add(selectedType);
                // Branch에 기록 업데이트
                branches[x].OnNodeSelected(selectedType, floor);
            }

            //  가로줄 잭팟(동일 특수방 3개) 방지
            if (!isSingleNodeFloor)
            {
                CheckAndFixJackpot(floorIndex, floor, floorTypes);
            }
        }
        
        // 3. 노드 간 연결(선) 생성
        GenerateFixedPaths();
    }
    
    // 특정 위치의 노드 타입을 결정하는 함수 (우선순위: 고정 > 천장 > 가중치 랜덤)
    private NodeType DetermineNodeType(Branch branch, int floor, Dictionary<NodeType, NodeTypeDataEntry> rules)
    {
        // 1. 고정 층 처리
        if (floor == 1) return NodeType.Monster;
        if (floor == 10) return NodeType.Shelter;
        if (floor == 19) return NodeType.Shelter;
        if (floor == 20) return NodeType.Boss;

        // 2. 천장(Pity) 체크: 너무 안 나온 타입은 후보 리스트에 추가
        List<NodeType> pityList = new List<NodeType>();
        foreach (var key in rules.Keys)
        {
            if (branch.CheckPity(key, floor)) pityList.Add(key);
        }
        // 천장 발동 시 그 중에서 랜덤 선택
        if (pityList.Count > 0) return pityList[Random.Range(0, pityList.Count)];

        // 3. 가중치(Ratio) 기반 랜덤 선택
        float totalWeight = 0f;
        foreach (var entry in rules.Values)
        {
            if (branch.CanSpawn(entry.type, floor)) totalWeight += entry.ratio;
        }

        // 생성 가능한 게 없으면 기본값(몬스터)
        if (totalWeight <= 0) return NodeType.Monster;

        // 랜덤 뽑기 (룰렛 방식)
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

    // 가로 3칸이 모두 같은 특수 방(예: 상점 3개)이 되는 것을 방지
    private void CheckAndFixJackpot(int floorIndex, int currentFloor, List<NodeType> types)
    {
        if (types.Count >= 3)
        {
            NodeType first = types[0];
            // 몬스터 3개는 괜찮음, 특수 방만 체크
            if (first != NodeType.Monster)
            {
                bool allSame = true;
                for(int i=1; i<types.Count; i++)
                {
                    if(types[i] != first) { allSame = false; break; }
                }

                // 모두 같다면 하나를 랜덤하게 몬스터로 변경
                if (allSame)
                {
                    int targetIdx = Random.Range(0, width);
                    mapGrid[floorIndex][targetIdx].nodeType = NodeType.Monster;
                    // Branch 기록도 롤백
                    branches[targetIdx].ModifyHistory(first, NodeType.Monster, currentFloor);
                }
            }
        }
    }

    // 층수에 따라 고정된 패턴으로 노드를 연결하는 함수
    private void GenerateFixedPaths()
    {
        // 1. 기존 연결 정보 초기화
        foreach (var row in mapGrid)
            foreach (var node in row)
            {
                node.nextNodes.Clear(); 
            }

        // 2. 바닥부터 올라가며 연결
        for (int y = 0; y < height - 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                MapNode currentNode = mapGrid[y][x];
                if (!currentNode.isActive) continue;

                // === 연결 패턴 규칙 ===
                // 1. 직선 구간 (일반 스테이지)
                if ((y >= 0 && y <= 7) || (y >= 10 && y <= 16))
                {
                    TryConnect(currentNode, x, y + 1); 
                }
                // 2. 모이는 구간 (보스/휴식 전) -> 중앙으로 모임
                else if (y == 8 || y == 17)
                {
                    TryConnect(currentNode, 1, y + 1); 
                }
                // 3. 퍼지는 구간 (휴식 후) -> 3갈래로 퍼짐
                else if (y == 9)
                {
                    if (x == 1) 
                    {
                        TryConnect(currentNode, 0, y + 1);
                        TryConnect(currentNode, 1, y + 1);
                        TryConnect(currentNode, 2, y + 1);
                    }
                }
                // 4. 보스 진입 (일직선)
                else if (y == 18)
                {
                    if (x == 1) TryConnect(currentNode, 1, y + 1);
                }
            }
        }
    }

    // 연결 시도 함수 (유효성 검사 포함)
    private void TryConnect(MapNode fromNode, int targetX, int targetY)
    {
        // 맵 범위 체크
        if (targetY >= height || targetX < 0 || targetX >= width) return;
        
        MapNode targetNode = mapGrid[targetY][targetX];
        
        // 목표 노드가 비활성 상태면 연결 안 함
        if (!targetNode.isActive) return;
        
        fromNode.nextNodes.Add(targetNode);
    }
 
    // 플레이어캐릭터 상호작용    
    // 노드(버튼) 클릭 시 호출되는 함수
    public void OnNodeClicked(MapNode targetNode)
    {
        // 1. 플레이어가 없거나 (활성화 된 상태에서) 이동 중이면 무시
        if (playerMove == null) return;
        if (playerMove.gameObject.activeSelf && playerMove.IsMoving) return;

        // 2. 갈 수 있는 곳인지 확인
        if (CheckIsPathValid(targetNode))
        {
            // 게임 시작 후 첫 클릭처리
            if (currentNode == null)
            {
                // 플레이어 활성화
                playerMove.gameObject.SetActive(true);
                
                // MapVisualizer를 통해 플레이어의 시작 위치를 클릭한 노드 근처로 이동                
                
                MapVisualizer.Instance.SetPlayerPositionDirectly(targetNode, new Vector3(0, -100f, 0)); // y값 오프셋                
            }

            // 3. 현재 노드 정보 업데이트
            currentNode = targetNode;

            // 4. 시각적 이동 요청           
            
            MapVisualizer.Instance.MovePlayer(targetNode);
            
            
            Debug.Log($"[MapManager] ({targetNode.x}, {targetNode.y})로 이동");
        }        
    }

    // 이동 유효성 검사 로직
    private bool CheckIsPathValid(MapNode targetNode)
    {
        // 1. 게임 시작 후 첫 선택 (1층 노드만 가능)
        if (currentNode == null)
        {
            if (targetNode.y == 0) return true;
            return false;
        }
        
        // 2. 현재 노드와 연결된 다음 노드인지 확인
        if (currentNode.nextNodes.Contains(targetNode))
        {
            return true;
        }
        
        return false;
    }
}