using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;


public class NodeVisualData
{
    public Image image;
    public Button button;
}

// =========================================================
// 맵의 UI 요소를 화면에 그리고 갱신하는 시각화 담당 매니저
// =========================================================
public class MapVisualizer : SceneSingleton<MapVisualizer>
{
    [Header("UI Settings")]
    [SerializeField] private Transform mapContentParent;
    [SerializeField] private GameObject linePrefab;
    [SerializeField] private ScrollRect mapScrollView;

    [Header("Node Prefabs")]
    [SerializeField] private GameObject normalStagePrefab;
    [SerializeField] private GameObject eliteStagePrefab;
    [SerializeField] private GameObject bossStagePrefab;
    [SerializeField] private GameObject shelterStagePrefab;
    [SerializeField] private GameObject storeStagePrefab;
    [SerializeField] private GameObject eventStagePrefab;

    [Header("Layout Settings")]
    [SerializeField] private float xSpacing = 200f; // 노드 간 가로 간격
    [SerializeField] private float ySpacing = 150f; // 노드 간 세로(층) 간격
    [SerializeField] private float startYPosition = -3800f; // 가장 아래(1층)의 시작 Y 좌표
    [SerializeField] private float fixedContentHeight = 6000f; // 스크롤 뷰 전체 맵의 높이

    private Transform lineContainer;
    private Transform nodeContainer;

    // 로직 데이터(MapNode)와 생성된 UI 오브젝트(GameObject)를 매칭시키는 딕셔너리
    private Dictionary<MapNode, GameObject> nodeObjMap = new Dictionary<MapNode, GameObject>();

    // 층별로 렌더링된 요소들을 관리 (지나간 층을 흐리게 만들 때 사용)
    private Dictionary<int, List<NodeVisualData>> nodesByFloor = new Dictionary<int, List<NodeVisualData>>();
    private Dictionary<int, List<Image>> linesByFloor = new Dictionary<int, List<Image>>();

    // Canvas 리빌드 렉을 막기 위한 오브젝트풀링 창고
    private Dictionary<NodeType, Queue<GameObject>> nodePools = new Dictionary<NodeType, Queue<GameObject>>();
    private Queue<GameObject> linePool = new Queue<GameObject>();
    private CancellationTokenSource scrollCts;

    private void Start()
    {
        if (MapManager.Instance == null) return;

        ShowMap(MapManager.Instance.mapGrid);
        ButtonEvent.Instance.ActiveMap();
    }

    // 맵 그리는 메인함수
    public void ShowMap(List<List<MapNode>> mapGrid)
    {
        if (mapGrid == null || mapGrid.Count == 0 || mapGrid[0].Count == 0) return;

        InitializeUI();
        ClearOldMap(); // 기존 맵 치우기

        int mapWidth = mapGrid[0].Count;

        // 1. 방(노드) 오브젝트 생성 배치
        foreach (var row in mapGrid)
        {
            foreach (var node in row)
            {
                if (node.isActive) CreateNodeObject(node, mapWidth);
            }
        }

        // 2. 방과 방 사이를 잇는 선 생성
        DrawConnections(mapGrid);

        // 3. 플레이어 아이콘 위치 초기화
        InitializePlayerParent();

        // 4. 스크롤 뷰를 1층(맨 밑)으로 세팅
        mapScrollView.verticalNormalizedPosition = 0f;
        mapScrollView.velocity = Vector2.zero;

        // 5. 클릭 불가능한 노드 비활성화 처리
        UpdateNodeInteractivity(MapManager.Instance.CurrentNode);
    }

    private void InitializeUI()
    {
        if (mapContentParent == null || mapScrollView == null) return;

        RectTransform contentRect = mapContentParent.GetComponent<RectTransform>();
        contentRect.pivot = new Vector2(0.5f, 0f);
        contentRect.anchorMin = new Vector2(0.5f, 0f);
        contentRect.anchorMax = new Vector2(0.5f, 0f);
        contentRect.anchoredPosition = Vector2.zero;
        contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, fixedContentHeight);

        // 깔끔한 계층 구조를 위해 선은 뒤, 노드는 앞에 나오도록 컨테이너 분리
        if (lineContainer == null) CreateContainer("LineContainer", out lineContainer);
        if (nodeContainer == null) CreateContainer("NodeContainer", out nodeContainer);
    }


    /// 이전 맵 데이터를 지울 때, 파괴(Destroy)하지 않고 창고(Pool)에 보관하여 재활용
    private void ClearOldMap()
    {
        foreach (var kvp in nodeObjMap)
        {
            kvp.Value.SetActive(false); // 오브젝트 끄기
            NodeType type = kvp.Key.nodeType;
            if (!nodePools.ContainsKey(type)) nodePools[type] = new Queue<GameObject>();
            nodePools[type].Enqueue(kvp.Value); // 노드 창고에 반납
        }
        nodeObjMap.Clear();
        nodesByFloor.Clear();

        foreach (var lines in linesByFloor.Values)
        {
            foreach (var line in lines)
            {
                line.gameObject.SetActive(false);
                linePool.Enqueue(line.gameObject); // 선 창고에 반납
            }
        }
        linesByFloor.Clear();
    }

    private void CreateContainer(string name, out Transform container)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(mapContentParent, false);
        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;
        rect.localScale = Vector3.one;
        container = go.transform;
    }


    /// 개별 노드 프리팹을 풀에서 꺼내거나 새로 만들어서 지정된 위치에 배치
    private void CreateNodeObject(MapNode node, int mapWidth)
    {
        GameObject prefab = GetPrefabByNodeType(node.nodeType);
        if (prefab == null)
        {
            Debug.LogWarning($"[MapVisualizer] {node.nodeType} 프리팹이 할당되지 않았습니다!");
            // 맵노드 프리펩 까먹고 안넣으면 경고용
            return;
        }

        GameObject newObj = null;

        // [오브젝트 풀링] 해당 타입의 프리팹이 창고에 남아있다면 꺼내옴
        if (nodePools.TryGetValue(node.nodeType, out Queue<GameObject> pool) && pool.Count > 0)
        {
            newObj = pool.Dequeue();
            newObj.SetActive(true);
        }
        else
        {
            newObj = Instantiate(prefab, nodeContainer);
        }

        newObj.transform.localScale = Vector3.one;

        // 맵 중앙을 기준으로 x, y 픽셀 위치 계산
        float xPos = (node.x - (mapWidth - 1) / 2.0f) * xSpacing;
        float yPos = startYPosition + (node.y * ySpacing);
        newObj.transform.localPosition = new Vector3(xPos, yPos, 0);

        nodeObjMap.Add(node, newObj);

        NodeVisualData visualData = new NodeVisualData();
        visualData.image = newObj.GetComponent<Image>();
        visualData.button = newObj.GetComponent<Button>();

        // 색상과 클릭 가능 여부 초기화
        if (visualData.image != null)
        {
            visualData.image.color = Color.white;
        }

        SetButtonInteractable(visualData.button, true);

        if (!nodesByFloor.ContainsKey(node.y)) nodesByFloor.Add(node.y, new List<NodeVisualData>());
        nodesByFloor[node.y].Add(visualData);

        // 이전 맵에서 연결됐던 이벤트 리스너를 지우고 현재 노드 데이터를 연결
        if (visualData.button != null)
        {
            visualData.button.onClick.RemoveAllListeners();
        }

        NodeButton nodeButton = newObj.GetComponent<NodeButton>();
        if (nodeButton != null)
        {
            nodeButton.AddOnClickEvent(() => MapManager.Instance.OnNodeClicked(node));
        }
    }

    // 선 그리는 로직
    private void DrawConnections(List<List<MapNode>> mapGrid)
    {
        foreach (var row in mapGrid)
        {
            foreach (var node in row)
            {
                if (!node.isActive || !nodeObjMap.ContainsKey(node)) continue;
                Vector3 startPos = nodeObjMap[node].transform.localPosition;

                foreach (var nextNode in node.nextNodes)
                {
                    if (nodeObjMap.TryGetValue(nextNode, out GameObject targetObj))
                    {
                        CreateLine(startPos, targetObj.transform.localPosition, node.y);
                    }
                }
            }
        }
    }

    // 두 지점(A, B)의 거리와 각도를 계산하여 선 이미지(UI)를 늘리고 회전
    private void CreateLine(Vector3 startPos, Vector3 endPos, int floorIndex)
    {
        // 선도 마찬가지로 창고(Pool)에 남은 게 있다면 꺼내옴
        GameObject line = linePool.Count > 0 ? linePool.Dequeue() : Instantiate(linePrefab, lineContainer);
        line.SetActive(true);

        RectTransform rect = line.GetComponent<RectTransform>();
        rect.pivot = new Vector2(0f, 0.5f); // 회전 기준점을 선의 왼쪽 끝으로 설정
        rect.localPosition = startPos;

        Vector3 dir = endPos - startPos;
        float distance = dir.magnitude; // 거리
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg; // 각도

        // Z축을 돌려 각도를 맞추고 길이를 늘림
        rect.localRotation = Quaternion.Euler(0, 0, angle);
        rect.sizeDelta = new Vector2(distance, rect.sizeDelta.y);

        Image lineImage = line.GetComponent<Image>();
        lineImage.color = Color.white;

        if (!linesByFloor.ContainsKey(floorIndex)) linesByFloor.Add(floorIndex, new List<Image>());
        linesByFloor[floorIndex].Add(lineImage);
    }

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

    // 현재 위치(currentNode)를 기준으로 다음 갈 수 없는 방들은 버튼 클릭 방지

    public void UpdateNodeInteractivity(MapNode currentNode)
    {
        // 전체 비활성화
        foreach (var kvp in nodeObjMap)
        {
            SetButtonInteractable(kvp.Value.GetComponent<Button>(), false);
        }

        // 시작 전이면 맨 아래 1층 방들만 활성화
        if (currentNode == null)
        {
            foreach (var kvp in nodeObjMap)
            {
                if (kvp.Key.y == 0 && kvp.Key.isActive)
                {
                    SetButtonInteractable(kvp.Value.GetComponent<Button>(), true);
                }
            }
        }
        else // 게임 중이면 내 현재 위치에서 이어진(nextNodes) 방들만 활성화
        {
            foreach (MapNode nextNode in currentNode.nextNodes)
            {
                if (nodeObjMap.TryGetValue(nextNode, out GameObject nextObj))
                {
                    SetButtonInteractable(nextObj.GetComponent<Button>(), true);
                }
            }
        }
    }

    // 플레이어 오브젝트를 목표 방으로 이동시키고, 지나간 방들은 흐리게 만들며 화면을 스크롤
    public void MovePlayer(MapNode targetNode)
    {
        if (MapManager.Instance == null || MapManager.Instance.playerMove == null) return;

        if (nodeObjMap.TryGetValue(targetNode, out GameObject targetObj))
        {
            PlayerMove player = MapManager.Instance.playerMove;
            Vector3 targetLocalPos = targetObj.transform.localPosition;
            targetLocalPos.z = 0;
            player.MoveTo(targetLocalPos);

            FadeOutFloorVisuals(targetNode.y - 1); // 방금 떠난 층을 흐리게 만듦

            // 플레이어를 따라 카메라(스크롤 뷰)가 부드럽게 따라올라가도록 비동기 호출
            FocusOnPlayerAsync().Forget();
        }
    }

    private void InitializePlayerParent()
    {
        if (MapManager.Instance.playerMove == null)
            MapManager.Instance.playerMove = FindFirstObjectByType<PlayerMove>(FindObjectsInactive.Include);

        var player = MapManager.Instance.playerMove;
        if (player == null || nodeContainer == null || mapScrollView == null || mapScrollView.content == null) return;

        player.transform.SetParent(nodeContainer, false);
        player.transform.localScale = Vector3.one;
        player.transform.localRotation = Quaternion.identity;

        Vector3 pos = player.transform.localPosition;
        pos.z = 0;
        player.transform.localPosition = pos;

        if (MapManager.Instance.CurrentNode != null)
        {
            if (nodeObjMap.TryGetValue(MapManager.Instance.CurrentNode, out GameObject targetObj))
            {
                player.transform.localPosition = targetObj.transform.localPosition;
                player.gameObject.SetActive(true);

                // 게임 로드 시에는 스크롤 애니메이션 없이 즉시 화면을 텔레포트 맞춤
                float targetContentY = -player.transform.localPosition.y - 1400f;
                mapScrollView.content.anchoredPosition = new Vector2(mapScrollView.content.anchoredPosition.x, targetContentY);

                int currentFloor = MapManager.Instance.CurrentNode.y;
                for (int i = 0; i < currentFloor; i++) FadeOutFloorVisuals(i);
            }
        }
        else
        {
            player.gameObject.SetActive(false); // 게임 시작 전엔 숨김
        }
    }

    public void SetPlayerPositionDirectly(MapNode targetNode, Vector3 offset)
    {
        if (MapManager.Instance == null || MapManager.Instance.playerMove == null) return;

        if (nodeObjMap.TryGetValue(targetNode, out GameObject targetObj))
        {
            PlayerMove player = MapManager.Instance.playerMove;
            player.transform.localPosition = targetObj.transform.localPosition + offset;
        }
    }


    // 플레이어가 층을 올라갈 때마다 지나간 아래 층들의 색상을 어둡게/반투명하게 처리

    public void FadeOutFloorVisuals(int targetFloor)
    {
        if (nodesByFloor.TryGetValue(targetFloor, out List<NodeVisualData> nodes))
        {
            foreach (var nodeData in nodes)
            {
                if (nodeData.image != null)
                {
                    nodeData.image.color = new Color(1f, 1f, 1f, 0.7f);
                }

                SetButtonInteractable(nodeData.button, false);
            }
        }

        if (linesByFloor.TryGetValue(targetFloor, out List<Image> lines))
        {
            foreach (var lineImg in lines)
            {
                lineImg.color = new Color(lineImg.color.r, lineImg.color.g, lineImg.color.b, 0.5f);
            }
        }
    }

    // [UniTask] 플레이어가 이동하면 화면 스크롤이 플레이어를 따라가게 부드럽게 감속하며 이동
    // 비동기로 처리

    private async UniTaskVoid FocusOnPlayerAsync()
    {
        if (MapManager.Instance == null || MapManager.Instance.playerMove == null || mapScrollView == null || mapScrollView.content == null) return;

        // 이전에 진행 중이던 스크롤 이동이 있다면 취소(Cancel)시켜 중복 충돌을 방지
        if (scrollCts != null)
        {
            scrollCts.Cancel();
            scrollCts.Dispose();
        }
        scrollCts = new CancellationTokenSource();
        CancellationToken token = scrollCts.Token;

        float playerY = MapManager.Instance.playerMove.transform.localPosition.y;
        float targetContentY = -playerY - 1400f; // 화면이 예쁘게 중앙에서 살짝 아래 잡히도록 오프셋 적용

        Vector2 startPos = mapScrollView.content.anchoredPosition;
        Vector2 targetPos = new Vector2(startPos.x, targetContentY);

        float duration = 0.5f; // 0.5초간 카메라 이동
        float time = 0f;

        while (time < duration)
        {
            if (token.IsCancellationRequested) return;

            time += Time.deltaTime;
            float t = time / duration;
            float smoothT = Mathf.Sin(t * Mathf.PI * 0.5f); // 점점 느려지는 감속(EaseOut) 연출 

            mapScrollView.content.anchoredPosition = Vector2.Lerp(startPos, targetPos, smoothT);

            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }

        if (!token.IsCancellationRequested)
            mapScrollView.content.anchoredPosition = targetPos; // 종료 시 완벽하게 목표 좌표 덮어쓰기
    }

    private static void SetButtonInteractable(Button button, bool isInteractable)
    {
        if (button != null)
        {
            button.interactable = isInteractable;
        }
    }

    private void OnDestroy()
    {
        // 씬이 파괴될 때 남아있는 비동기 로직이 에러를 일으키지 않게 토큰 회수
        if (scrollCts != null)
        {
            scrollCts.Cancel();
            scrollCts.Dispose();
        }
    }
}