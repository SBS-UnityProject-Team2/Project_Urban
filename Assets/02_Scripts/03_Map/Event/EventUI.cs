using System.Collections;
using UnityEngine;

/// <summary>
/// ╔════════════════════════════════════════════════════════════════════╗
/// ║                    EVENT UI (컨트롤러 계층)                          ║
/// ╚════════════════════════════════════════════════════════════════════╝
/// 
/// 📋 역할:
///   • 이벤트 데이터 조회 및 전달
///   • Panel_Script와 Panel_Choice에 데이터 분배
///   • 이벤트 흐름 제어 (시작 → 스크립트 표시 → 선택지 표시 → 종료)
/// 
/// 🔄 새로운 구조:
///   EventUI (데이터 중계만)
///   ├─ Panel_Script (EventScriptText) - 스크립트/대사/일러스트 출력
///   └─ Panel_Choice (Button1, 2, 3) - 선택지 처리
/// </summary>
public class EventUI : MonoBehaviour
{
    public static EventUI Instance;

    [Header("Panel References")]
    [SerializeField] private EventScriptText panelScript;  // Panel_Script
    [SerializeField] private EventButtonUI button1;        // Panel_Choice의 버튼들
    [SerializeField] private EventButtonUI button2;
    [SerializeField] private EventButtonUI button3;

    [Header("Event Root")]
    [SerializeField] private GameObject eventPanelRoot;

    private EventScript.EventInfo currentEventInfo;

    private void Awake()
    {
        Instance = this;
    }

    // =================================================================
    // [이벤트 시작] EventCode로 데이터 조회 후 Panel_Script에 전달
    // =================================================================
    public void BeginEvent(int eventCode)
    {
        // 이벤트 정보 조회
        currentEventInfo = EventManager.Instance.GetEventInfo(eventCode);

        // 스크립트 정보 조회
        var scriptInfo = EventManager.Instance.GetScriptInfo(currentEventInfo.EventScript);

        // 이벤트 패널 활성화
        eventPanelRoot.SetActive(true);

        // 선택지 버튼 초기 비활성화
        HideChoices();

        // Panel_Script에 데이터 전달 (타이핑은 Panel_Script가 담당)
        // Panel_Script GameObject가 비활성화되어 있으면 활성화
        if (!panelScript.gameObject.activeInHierarchy)
        {
            panelScript.gameObject.SetActive(true);
        }

        // 활성화 확인 후 코루틴 시작
        if (panelScript.gameObject.activeInHierarchy)
        {
            panelScript.SetupScript(
                scriptInfo.Dialogue,
                scriptInfo.EventScript,
                scriptInfo.Illustration
            );
        }


        // 타이핑 완료 후 OnScriptComplete()가 호출됨
    }

    // =================================================================
    // [스크립트 완료 콜백] Panel_Script의 타이핑이 끝나면 호출됨
    // =================================================================
    public void OnScriptComplete()
    {
        // Panel_Choice의 버튼들에 데이터 전달
        BuildChoices();
    }

    // =================================================================
    // [선택지 표시] 버튼들에 choiceCode 할당
    // =================================================================
    private void HideChoices()
    {
        EventButtonUI[] buttons = { button1, button2, button3 };
        foreach (var btn in buttons)
        {
            btn.gameObject.SetActive(false);
        }
    }

    private void ShowChoices()
    {
        EventButtonUI[] buttons = { button1, button2, button3 };
        foreach (var btn in buttons)
        {
            btn.gameObject.SetActive(true);
        }
    }

    private void BuildChoices()
    {
        int[] choiceCodes = { 
            currentEventInfo.EventChoice1, 
            currentEventInfo.EventChoice2, 
            currentEventInfo.EventChoice3 
        };

        EventButtonUI[] buttons = { button1, button2, button3 };

        for (int i = 0; i < 3; i++)
        {
            buttons[i].Setup(choiceCodes[i], this);
        }

        // 선택지 활성화 (덬! 하고 나타나게)
        ShowChoices();
    }

    // =================================================================
    // [결과 스크립트 표시] EventButtonUI가 선택 후 호출
    // =================================================================
    public void ShowResultScript(int scriptCode, int resultCode, string selectedCardName = "")
    {
        // 선택지 버튼 완전히 숨기기
        HideChoices();

        var resultScript = EventManager.Instance.GetResultScriptInfo(scriptCode);
        var rewardData = EventManager.Instance.GetRewardInfo(resultCode);
        
        // EndScript 플레이스홀더 치환
        string formattedEndScript = RewardTextFormatter.FormatEndScript(
            resultScript.EndScript, 
            rewardData,
            selectedCardName
        );

        panelScript.SetupResultScript(
            resultScript.Dialogue,
            resultScript.ResultScript,
            formattedEndScript
        );

        // 결과 스크립트만 표시 (나가기 버튼으로 직접 종료)
    }

    // =================================================================
    // [이벤트 종료]
    // =================================================================
    // 주석: 나가기 버튼으로만 종료하도록 변경
    /*
    private IEnumerator CloseEventAfterInput()
    {
        // 입력 대기
        yield return new WaitUntil(() => 
            Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)
        );

        // 이벤트 패널 닫기
        if (eventPanelRoot != null)
            eventPanelRoot.SetActive(false);
    }
    */
}
