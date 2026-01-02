using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MapVisualizer : SceneSingleton<MapVisualizer>
{

    [Header("UI References")]
    [SerializeField] private Transform mapContentParent; // 맵 오브젝트들이 생성될 부모 Transform (Scroll View의 Content)
    [SerializeField] private GameObject linePrefab;      // 노드 사이를 이어줄 선 프리팹
    [SerializeField] private GameObject playerImagePrefab;  // 플레이어 캐릭터 프리펩

    [Header("Node Prefabs")]
    // 각 노드 타입에 매칭되는 UI 프리팹들
    [SerializeField] private GameObject normalStagePrefab;
    [SerializeField] private GameObject eliteStagePrefab;
    [SerializeField] private GameObject bossStagePrefab;
    [SerializeField] private GameObject shelterStagePrefab;
    [SerializeField] private GameObject storeStagePrefab;
    [SerializeField] private GameObject eventStagePrefab;

    [Header("Layout Settings")]
    [SerializeField] private float xSpacing = 200f;      // 노드 간 가로 간격
    [SerializeField] private float ySpacing = 150f;      // 노드 간 세로 간격
    [SerializeField] private float startYPosition = -3800f; // 1층 노드가 생성될 시작 Y 좌표
    [SerializeField] private float fixedContentHeight = 6000f; // 스크롤 뷰의 전체 높이 고정값

    // 계층 구조 정리용 컨테이너 (선은 뒤에, 노드는 앞에 그리기 위함)
    private Transform lineContainer;
    private Transform nodeContainer;

    // MapNode와 실제 화면 GameObject를 연결해주는 캐시
    private Dictionary<MapNode, GameObject> nodeObjMap = new Dictionary<MapNode, GameObject>();

    private void Start()
    {
        ShowMap(MapManager.Instance.mapGrid);
        ButtonEvent.Instance.ActiveMap();
    }

    // 외부에서 맵 생성이 완료되면 호출하는 메인 함수
    public void ShowMap(List<List<MapNode>> mapGrid)
    {
        if (mapGrid == null || mapGrid.Count == 0) return;

        // 1. 기존 맵 삭제 및 UI 설정 초기화
        InitializeUI();
        ClearOldMap();

        int mapWidth = mapGrid[0].Count;

        // 2. 노드 오브젝트 생성
        foreach (var row in mapGrid)
        {
            foreach (var node in row)
            {
                // 활성화된 노드이고, 타입이 None이 아닐 때만 그림
                if (node.isActive)
                {
                    CreateNodeObject(node, mapWidth);
                }
            }
        }

        // 3. 선 그리기
        DrawConnections(mapGrid);

        InitializePlayerParent();

        // 4. 스크롤 위치 초기화 (맨 아래 1층부터 시작하도록)
        // StartCoroutine(ResetScroll());

        ScrollRect sr = mapContentParent.GetComponentInParent<ScrollRect>();

        sr.verticalNormalizedPosition = 0f; // 0 = 바닥, 1 = 천장
        sr.velocity = Vector2.zero;         // 관성 제거
    }

    // ScrollView의 Content 크기 및 피벗 설정
    private void InitializeUI()
    {
        RectTransform contentRect = mapContentParent.GetComponent<RectTransform>();
        contentRect.pivot = new Vector2(0.5f, 0f);      // 피벗을 하단 중앙으로 설정
        contentRect.anchorMin = new Vector2(0.5f, 0f);
        contentRect.anchorMax = new Vector2(0.5f, 0f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, fixedContentHeight);
    }

    // 이전에 생성된 노드와 선들을 모두 제거하고 컨테이너를 재생성
    private void ClearOldMap()
    {
        foreach (Transform child in mapContentParent) Destroy(child.gameObject);
        nodeObjMap.Clear();

        // 선이 노드보다 뒤에 그려지도록 순서 관리 
        CreateContainer("LineContainer", out lineContainer);
        CreateContainer("NodeContainer", out nodeContainer);
    }

    // 계층 정리를 위한 빈 오브젝트 생성 헬퍼 함수
    private void CreateContainer(string name, out Transform container)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(mapContentParent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        // 부모 영역을 가득 채우도록 설정
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;

        container = go.transform;
    }

    // 개별 노드(아이콘) 생성 함수
    private void CreateNodeObject(MapNode node, int mapWidth)
    {
        GameObject prefabToUse = GetPrefabByNodeType(node.nodeType);
        if (prefabToUse == null) throw new System.Exception();

        GameObject newObj = Instantiate(prefabToUse, nodeContainer);
        newObj.transform.localScale = Vector3.one;

        // [위치 계산 로직]
        // x: 중앙 정렬을 위해 (현재 인덱스 - (전체폭-1)/2) 공식을 사용
        // y: 시작 위치 + 층수 * 간격
        float xPos = (node.x - (mapWidth - 1) / 2.0f) * xSpacing;
        float yPos = startYPosition + (node.y * ySpacing);

        newObj.transform.localPosition = new Vector3(xPos, yPos, 0);

        // 나중에 선을 그을 때 이 노드의 위치를 찾아야 하므로 딕셔너리에 등록
        nodeObjMap.Add(node, newObj);

        // 버튼클릭 이벤트 연결
        NodeButton btn = newObj.GetComponent<NodeButton>();
        btn.AddOnClickEvent(() => MapManager.Instance.OnNodeClicked(node));
    }

    // 데이터에 저장된 연결 정보를 시각화하는 함수
    private void DrawConnections(List<List<MapNode>> mapGrid)
    {
        foreach (var row in mapGrid)
        {
            foreach (var node in row)
            {
                // 화면에 없는 노드는 건너뜀
                if (!node.isActive || !nodeObjMap.ContainsKey(node)) continue;

                Vector3 startPos = nodeObjMap[node].transform.localPosition;

                // MapNode 데이터 안에 이미 누구랑 연결될지(nextNodes)가 들어있음
                // 단순히 그 리스트를 순회하며 선만 그어주면 됨
                foreach (var nextNode in node.nextNodes)
                {
                    // 목적지 노드가 실제로 화면에 존재한다면 선 생성
                    if (nodeObjMap.TryGetValue(nextNode, out GameObject targetObj))
                    {
                        CreateLine(startPos, targetObj.transform.localPosition);
                    }
                }
            }
        }
    }

    // 두 점 사이를 잇는 선 오브젝트 생성 및 배치
    private void CreateLine(Vector3 startPos, Vector3 endPos)
    {
        GameObject line = Instantiate(linePrefab, lineContainer);
        RectTransform rect = line.GetComponent<RectTransform>();

        // 1. 시작점에 피벗을 맞춤 (회전의 기준점이 됨)
        rect.pivot = new Vector2(0f, 0.5f);
        rect.localPosition = startPos;

        // 2. 거리(선의 길이)와 각도(회전) 계산
        Vector3 dir = endPos - startPos;
        float distance = dir.magnitude;

        // Atan2를 사용하여 두 점 사이의 각도를 구함 (라디안 -> 디그리 변환)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 3. 회전 및 크기 적용
        rect.localRotation = Quaternion.Euler(0, 0, angle);
        rect.sizeDelta = new Vector2(distance, rect.sizeDelta.y); // 너비=거리, 높이=선 두께
    }

    // 노드 타입에 맞는 프리팹 반환
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

    // 맵 생성 직후 스크롤 위치를 맨 아래로 초기화
    private IEnumerator ResetScroll()
    {
        yield return null; // 한 프레임 대기 (UI 갱신 대기)

        ScrollRect sr = mapContentParent.GetComponentInParent<ScrollRect>();

        sr.verticalNormalizedPosition = 0f; // 0 = 바닥, 1 = 천장
        sr.velocity = Vector2.zero;         // 관성 제거
    }

    // 플레이어 캐릭터를 클릭한 노드 위치로 이동시키는 함수
    public void MovePlayer(MapNode targetNode)
    {
        if (nodeObjMap.TryGetValue(targetNode, out GameObject targetObj))
        {
            PlayerMove player = MapManager.Instance.playerMove;

            if (player != null)
            {
                // 타겟(노드)의 로컬 좌표 가져오기
                Vector3 targetLocalPos = targetObj.transform.localPosition;

                // Z값은 0으로 고정
                targetLocalPos.z = 0;

                // 이동 명령
                player.MoveTo(targetLocalPos);
                UpdatePastNodesVisual(targetNode.y);
            }
        }
    }

// 플레이어프리펩 노드컨테이너 밑에 생성
   private void InitializePlayerParent()
{
    // 1. 플레이어 찾기
    if (MapManager.Instance.playerMove == null)
    {
        MapManager.Instance.playerMove = FindFirstObjectByType<PlayerMove>(FindObjectsInactive.Include);
    }

    var player = MapManager.Instance.playerMove;

    player.transform.SetParent(nodeContainer, false); 
    player.transform.localScale = Vector3.one;
    player.transform.localRotation = Quaternion.identity;

    Vector3 pos = player.transform.localPosition;
    pos.z = 0;
    player.transform.localPosition = pos;

    // 2. 배틀에서 돌아왔을 때 처리
    if (MapManager.Instance.CurrentNode != null)
    {
        // 딕셔너리에서 노드 오브젝트를 가져와서 바로 위치 이동
        if (nodeObjMap.TryGetValue(MapManager.Instance.CurrentNode, out GameObject targetObj))
        {
            player.transform.localPosition = targetObj.transform.localPosition;                    
            player.gameObject.SetActive(true);
            UpdatePastNodesVisual(MapManager.Instance.CurrentNode.y);   // 현재층보다 낮은 노드를 흐릿하게 처리
        }
    }
    else
    {
        player.gameObject.SetActive(false);
    }
}
    // 플레이어 프리펩 초기위치 지정용
    public void SetPlayerPositionDirectly(MapNode targetNode, Vector3 offset)
    {
        if (nodeObjMap.TryGetValue(targetNode, out GameObject targetObj))
        {
            PlayerMove player = MapManager.Instance.playerMove;

            // 타겟 노드의 위치 가져오기
            Vector3 targetPos = targetObj.transform.localPosition;

            // 원하는 오프셋만큼 더해서 시작 위치 설정 
            player.transform.localPosition = targetPos + offset;

        }
    }

    // 지나온 노드들을 흐릿하게 만드는 함수
    public void UpdatePastNodesVisual(int currentFloor)
    {
        // 생성된 모든 노드 오브젝트(nodeObjMap)를 하나씩 검사
        foreach (var kvp in nodeObjMap)
        {
            MapNode node = kvp.Key;       // 데이터 (좌표 정보)
            GameObject obj = kvp.Value;   // 실제 게임 오브젝트 (UI)

            // UI 이미지와 버튼 컴포넌트 가져오기
            var img = obj.GetComponent<UnityEngine.UI.Image>();
            var btn = obj.GetComponent<UnityEngine.UI.Button>();

            if (img == null) continue;

            // 로직: 노드의 층(y)이 현재 층보다 작으면 = 지나온 층
            if (node.y < currentFloor)
            {
                // 1. 색상을 어둡고 투명하게 변경 (반투명)
                img.color = new Color(1f, 1f, 1f, 0.7f); // 흰색에 투명도 50%
                
                // 2. 버튼 클릭 비활성화 (이미 지나온 곳은 못 누르게)
                if (btn != null) btn.interactable = false;
            }
            else
            {
                // 현재 층이거나 미래의 층은 원래대로 밝게
                img.color = Color.white;
                
                // (선택) 버튼은 원래 로직대로 활성화
                if (btn != null) btn.interactable = true; 
            }
        }
    }
    
}