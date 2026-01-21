using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Controller] 이벤트 시스템의 흐름을 제어합니다. (MVC 패턴의 Controller)
/// - 데이터(Model)를 로드하고 조회합니다.
/// - 화면(View)에게 무엇을 그릴지 명령합니다.
/// - 실제 보상 로직은 버튼에게 위임했습니다.
/// </summary>
public class EventManager : Singleton<EventManager>
{
    [Header("1. Data Models")]
    [SerializeField] private EventScript eventDataSO;  
    [SerializeField] private EventChoice eventChoiceSO; 
    [SerializeField] private EventResult eventResultSO; 

    [Header("2. View Reference")]
    [SerializeField] private EventUI eventUI;          
    [SerializeField] private GameObject eventPanel;    

    protected override void Awake()
    {
        base.Awake();
        // 게임 시작 시 Dictionary 캐싱 (최적화)
        eventDataSO.Initialize();
        eventChoiceSO.Initialize();
        eventResultSO.Initialize();

        eventPanel.SetActive(false);
    }

    // 외부(노드 등)에서 호출하여 랜덤 이벤트 실행
    public void StartRandomEvent()
    {
        List<int> keys = eventDataSO.GetAllEventKeys();
        // 랜덤 키 추출 후 이벤트 시작
        StartEvent(keys[Random.Range(0, keys.Count)]);
    }

    // 특정 이벤트 ID로 실행
    public void StartEvent(int eventCode)
    {
        var eventInfo = eventDataSO.GetEvent(eventCode);
        
        eventPanel.SetActive(true);

        // 초기 대사 데이터 로드
        var scriptInfo = eventDataSO.GetScript(eventInfo.EventScript);
        
        // UI 초기화 및 대사 출력 명령
        eventUI.SetupUI(scriptInfo.Dialogue, scriptInfo.Illustration);
        eventUI.PlayTypeWriter(scriptInfo.EventScript, () => 
        {
            // 대사 출력이 끝나면 선택지 표시
            ShowChoices(eventInfo);
        });
    }

    // 선택지 버튼 생성 로직
    private void ShowChoices(EventScript.EventInfo eventInfo)
    {
        eventUI.ClearButtons();

        // 3개의 선택지 슬롯 순회
        int[] choiceCodes = { eventInfo.EventChoice1, eventInfo.EventChoice2, eventInfo.EventChoice3 };

        foreach (int code in choiceCodes)
        {
            if (code == 0) continue; // 빈 슬롯 스킵

            var choiceData = eventChoiceSO.GetChoice(code);
            var rewardData = eventResultSO.GetReward(choiceData.ResultCode);

            // 버튼 생성 시, '데이터'와 '완료 시 연락받을 콜백'을 함께 전달
            eventUI.CreateButton(choiceData, rewardData, OnButtonProcessFinished);
        }
    }

    // 버튼이 클릭되고 보상 지급까지 마쳤을 때 호출되는 콜백
    private void OnButtonProcessFinished(int nextScriptCode)
    {
        eventUI.ClearButtons(); // 선택지 제거

        // 결과 대사가 있다면 출력
        if (nextScriptCode != 0)
        {
            var resultScript = eventResultSO.GetResultScript(nextScriptCode);
            
            eventUI.SetupUI(resultScript.Dialogue, null); // 이미지는 유지
            eventUI.PlayTypeWriter(resultScript.ResultScript, () => 
            {
                StartCoroutine(EndEventRoutine());
            });
        }
        else
        {
            // 결과 대사가 없으면 즉시 종료
            StartCoroutine(EndEventRoutine());
        }
    }

    private IEnumerator EndEventRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0)); // 유저 확인 대기
        
        eventPanel.SetActive(false);
        Debug.Log("[EventManager] 이벤트 종료");
        // TODO: 맵 매니저에게 노드 클리어 신호 전달
    }
}