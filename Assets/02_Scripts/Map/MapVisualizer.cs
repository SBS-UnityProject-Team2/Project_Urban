using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeVisualData
{
    public GameObject obj;      // 노드 게임 오브젝트
    public Image image;         // 색상 변경용 이미지 컴포넌트
    public Button button;       // 클릭 제어용 버튼 컴포넌트
}

public class MapVisualizer : SceneSingleton<MapVisualizer>
{
    
    [Header("UI Settings")]    
    [SerializeField] private Transform mapContentParent;    
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private GameObject playerImagePrefab;
    [SerializeField] private ScrollRect mapScrollView;
   

    
    [Header("Node Prefabs")]
    [SerializeField] private GameObject normalStagePrefab;
    [SerializeField] private GameObject eliteStagePrefab;
    [SerializeField] private GameObject bossStagePrefab;
    [SerializeField] private GameObject shelterStagePrefab;
    [SerializeField] private GameObject storeStagePrefab;
    [SerializeField] private GameObject eventStagePrefab;
    

    
    [Header("Layout Settings")]

    [SerializeField] private float xSpacing = 200f;
    [SerializeField] private float ySpacing = 150f;
    [SerializeField] private float startYPosition = -3800f;
    [SerializeField] private float fixedContentHeight = 6000f;  
  

    // 계층 구조정리를 위한 컨테이너 (선은 뒤에, 노드는 앞에 그리기 위함)
    private Transform lineContainer;
    private Transform nodeContainer;

    // MapNode 데이터와 실제 게임오브젝트를 연결하는 딕셔너리 
    private Dictionary<MapNode, GameObject> nodeObjMap = new Dictionary<MapNode, GameObject>();

    // Key: 층수(y), Value: 해당 층에 있는 노드들의 컴포넌트 정보 리스트
    private Dictionary<int, List<NodeVisualData>> nodesByFloor = new Dictionary<int, List<NodeVisualData>>();

    // Key: 시작 층(y), Value: 해당 층에서 시작하는 선들의 Image 컴포넌트 리스트
    private Dictionary<int, List<Image>> linesByFloor = new Dictionary<int, List<Image>>();


    private void Start()
    {
        // 씬이 로드될 때 MapManager에 데이터가 있다면 맵 생성
        ShowMap(MapManager.Instance.mapGrid);

        // 버튼 이벤트 활성화 (필요한 경우)
        ButtonEvent.Instance.ActiveMap();

        // 플레이어 위치 초기화 및 상태 복구
        InitializePlayerParent();
    }

    // 외부(MapManager)에서 맵 생성이 완료되면 호출하여 UI를 그리는 메인 함수
    public void ShowMap(List<List<MapNode>> mapGrid)
    {
        // 1. 기존 맵 데이터 및 UI 오브젝트 전체 초기화
        InitializeUI();
        ClearOldMap();

        int mapWidth = mapGrid[0].Count;

        // 2. 노드 오브젝트 생성 및 배치
        foreach (var row in mapGrid)
        {
            foreach (var node in row)
            {
                // 활성화된 유효 노드만 생성
                if (node.isActive)
                {
                    CreateNodeObject(node, mapWidth);
                }
            }
        }

        // 3. 노드 간 연결 선 그리기
        DrawConnections(mapGrid);

        // 4. 플레이어 위치 잡기 (생성 직후에는 초기화 로직 실행)
        InitializePlayerParent();

        // 5. 스크롤 뷰 초기 상태 설정 (맨 아래)                
        mapScrollView.verticalNormalizedPosition = 0f; // 0: 바닥, 1: 천장
        mapScrollView.velocity = Vector2.zero;         // 관성 제거
        
    }


    // ScrollView Content의 크기 및 앵커를 설정

    private void InitializeUI()
    {
        RectTransform contentRect = mapContentParent.GetComponent<RectTransform>();
        contentRect.pivot = new Vector2(0.5f, 0f);      // 피벗: 하단 중앙
        contentRect.anchorMin = new Vector2(0.5f, 0f);
        contentRect.anchorMax = new Vector2(0.5f, 0f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, fixedContentHeight); // 높이 고정
    }


    // 이전에 생성된 모든 맵 오브젝트를 제거하고 자료구조 비움
    private void ClearOldMap()
    {
        // 기존 자식 오브젝트(노드, 선 등) 모두 파괴
        foreach (Transform child in mapContentParent) Destroy(child.gameObject);
        
        // 자료구조 초기화
        nodeObjMap.Clear();
        nodesByFloor.Clear(); 
        linesByFloor.Clear();  

        // UI 계층 분리 (선은 뒤에, 노드는 앞에)
        CreateContainer("LineContainer", out lineContainer);
        CreateContainer("NodeContainer", out nodeContainer);
    }


    // 노드나 선을 담을 컨테이너 오브젝트를 생성
    private void CreateContainer(string name, out Transform container)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(mapContentParent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        // 부모 영역  채우기
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;

        container = go.transform;
    }


    // 개별 노드를 생성
    private void CreateNodeObject(MapNode node, int mapWidth)
    {
        GameObject prefabToUse = GetPrefabByNodeType(node.nodeType);
        
        // 1. 프리팹 생성
        GameObject newObj = Instantiate(prefabToUse, nodeContainer);
        newObj.transform.localScale = Vector3.one;

        // 2. 위치 계산 (중앙 정렬 공식)
        float xPos = (node.x - (mapWidth - 1) / 2.0f) * xSpacing;
        float yPos = startYPosition + (node.y * ySpacing);
        newObj.transform.localPosition = new Vector3(xPos, yPos, 0);

        // 3. 기본 매핑 (로직용)
        nodeObjMap.Add(node, newObj);

        // 4. 시각 정보 캐싱
        NodeVisualData visualData = new NodeVisualData();
        visualData.obj = newObj;
        visualData.image = newObj.GetComponent<Image>();
        visualData.button = newObj.GetComponent<Button>();

        // 층별 딕셔너리에 등록
        if (!nodesByFloor.ContainsKey(node.y))
        {
            nodesByFloor.Add(node.y, new List<NodeVisualData>());
        }
        nodesByFloor[node.y].Add(visualData);

        // 5. 클릭 이벤트 연결
        NodeButton nodeBtnScript = newObj.GetComponent<NodeButton>();
        nodeBtnScript.AddOnClickEvent(() => MapManager.Instance.OnNodeClicked(node));
    }


    // 노드 간의 연결 선 그리기
    private void DrawConnections(List<List<MapNode>> mapGrid)
    {
        foreach (var row in mapGrid)
        {
            foreach (var node in row)
            {
                // Dictionary ContainsKey는 로직 필수 요소이므로 유지 (방어적 널체크 아님)
                if (!node.isActive || !nodeObjMap.ContainsKey(node)) continue;

                Vector3 startPos = nodeObjMap[node].transform.localPosition;

                foreach (var nextNode in node.nextNodes)
                {
                    if (nodeObjMap.TryGetValue(nextNode, out GameObject targetObj))
                    {
                        // 선 생성 및 층 정보 등록 (node.y 층에서 시작하는 선)
                        CreateLine(startPos, targetObj.transform.localPosition, node.y);
                    }
                }
            }
        }
    }


    // 두 지점 사이에 선을 생성하고 회전
    private void CreateLine(Vector3 startPos, Vector3 endPos, int floorIndex)
    {
        GameObject line = Instantiate(linePrefab, lineContainer);
        RectTransform rect = line.GetComponent<RectTransform>();

        // 1. 피벗 및 위치 설정
        rect.pivot = new Vector2(0f, 0.5f);
        rect.localPosition = startPos;

        // 2. 거리 및 각도 계산
        Vector3 dir = endPos - startPos;
        float distance = dir.magnitude;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 3. 변형 적용
        rect.localRotation = Quaternion.Euler(0, 0, angle);
        rect.sizeDelta = new Vector2(distance, rect.sizeDelta.y);

        // 4. 선 이미지 캐싱 (나중에 색 바꿀 때 사용)
        Image lineImage = line.GetComponent<Image>();
        
        if (!linesByFloor.ContainsKey(floorIndex))
        {
            linesByFloor.Add(floorIndex, new List<Image>());
        }
        linesByFloor[floorIndex].Add(lineImage);
    }


    /// 노드 타입에 맞는 프리팹을 반환
    private GameObject GetPrefabByNodeType(NodeType type)
    {
        return type switch
        {
            NodeType.Monster => normalStagePrefab,
            NodeType.Elite => eliteStagePrefab,
            NodeType.Boss => bossStagePrefab,
            NodeType.Shelter => shelterStagePrefab,
            NodeType.Store => storeStagePrefab,
            NodeType.Event => eventStagePrefab,
            _ => null,
        };
    }

    // [플레이어 이동 및 시각 효과 로직]
    // 플레이어를 특정 노드로 이동
    public void MovePlayer(MapNode targetNode)
    {
        // TryGetValue는 로직 흐름상 필수이므로 유지
        if (nodeObjMap.TryGetValue(targetNode, out GameObject targetObj))
        {
            PlayerMove player = MapManager.Instance.playerMove;

            // 타겟 위치로 이동 (Z값 보정)
            Vector3 targetLocalPos = targetObj.transform.localPosition;
            targetLocalPos.z = 0;
            player.MoveTo(targetLocalPos);

            // 방금 떠나온 층(현재 층 - 1)을 흐리게 처리
            FadeOutFloorVisuals(targetNode.y - 1);
            
            // 스크롤 위치 조정
            FocusOnPlayer();
        }
    }


    // 플레이어 오브젝트를 찾고 초기 위치와 상태를 설정
    private void InitializePlayerParent()
    {
        // 1. 플레이어가 없으면 찾기 (비활성화 상태 포함)
        // 여기서는 초기화 로직이므로 null일 때 찾는 과정은 유지, 이후 방어 코드는 삭제
        if (MapManager.Instance.playerMove == null)
        {
            MapManager.Instance.playerMove = FindFirstObjectByType<PlayerMove>(FindObjectsInactive.Include);
        }

        var player = MapManager.Instance.playerMove;
        
        // 2. 부모 및 Transform 초기화
        player.transform.SetParent(nodeContainer, false);
        player.transform.localScale = Vector3.one;
        player.transform.localRotation = Quaternion.identity;

        // Z값 0으로 평탄화
        Vector3 pos = player.transform.localPosition;
        pos.z = 0;
        player.transform.localPosition = pos;

        // 3. 게임 진행 상태에 따른 처리
        if (MapManager.Instance.CurrentNode != null)
        {
            // 저장된 현재 노드 위치로 플레이어 강제 이동
            if (nodeObjMap.TryGetValue(MapManager.Instance.CurrentNode, out GameObject targetObj))
            {
                player.transform.localPosition = targetObj.transform.localPosition;
                player.gameObject.SetActive(true);

                // 스크롤 위치 맞춤
                FocusOnPlayer();

                // 0층부터 (현재층-1)층까지 반복
                int currentFloor = MapManager.Instance.CurrentNode.y;
                for (int i = 0; i < currentFloor; i++)
                {
                    FadeOutFloorVisuals(i);
                }
            }
        }
        else
        {
            // 시작 전이라면 플레이어 숨김
            player.gameObject.SetActive(false);
        }
    }


    // 플레이어 시작 위치를 강제로 지정
    public void SetPlayerPositionDirectly(MapNode targetNode, Vector3 offset)
    {
        if (nodeObjMap.TryGetValue(targetNode, out GameObject targetObj))
        {
            PlayerMove player = MapManager.Instance.playerMove;
            player.transform.localPosition = targetObj.transform.localPosition + offset;
        }
    }


    // [시각 효과 최적화 함수]

    //특정 층(targetFloor)에 있는 노드와 선들을 흐리게(반투명/비활성)
    // Dictionary를 사용하여 O(1)로 해당 층 데이터에 접근
    public void FadeOutFloorVisuals(int targetFloor)
    {
        // 1. 노드 처리
        if (nodesByFloor.TryGetValue(targetFloor, out List<NodeVisualData> nodes))
        {
            foreach (var nodeData in nodes)
            {
                // 미리 캐싱된 컴포넌트 사용 
                nodeData.image.color = new Color(1f, 1f, 1f, 0.7f); // 반투명
                nodeData.button.interactable = false; // 클릭 불가
            }
        }

        // 2. 선(Line) 처리
        if (linesByFloor.TryGetValue(targetFloor, out List<Image> lines))
        {
            foreach (var lineImg in lines)
            {
                lineImg.color = new Color(1f, 1f, 1f, 0.5f); // 선은 좀 더 투명하게
            }
        }
    }


    // 플레이어 위치에 맞춰 스크롤 뷰를 이동
    // 공식: Content Y = -PlayerY - 1400f
    private void FocusOnPlayer()
    {
        // 플레이어 Y좌표
        float playerY = MapManager.Instance.playerMove.transform.localPosition.y;

        // 목표 Content Y좌표 계산
        float targetContentY = -playerY - 1400f;

        // 위치 적용
        Vector2 finalPos = mapScrollView.content.anchoredPosition;
        finalPos.y = targetContentY;
        mapScrollView.content.anchoredPosition = finalPos;
    }
}