using UnityEngine;
using UnityEngine.UI;

public class EventUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private EventScriptUI eventScriptUI;
    [SerializeField] private EventButtonsUI eventButtonsUI;
    [SerializeField] private EventRewardUI eventRewardUI;
    [SerializeField] private Button exitButton;

    private EventInfo currentEvent;

    private void Awake()
    {
        exitButton.onClick.AddListener(ButtonEvent.Instance.OnClickEventExit);    
    }

    private void OnEnable()
    {
        eventScriptUI.Init();
        eventButtonsUI.Init();
        eventRewardUI.Init();
        exitButton.gameObject.SetActive(false);

        currentEvent = EventManager.Instance.GetRandomEvent();

        eventScriptUI.gameObject.SetActive(true);
        eventScriptUI.StartEventScript(currentEvent.scriptCode, HandleEndEventScript);
    }

    // 이벤트 스크립트 재생이 끝나고 선택지 버튼 활성화
    private void HandleEndEventScript()
    {
        eventButtonsUI.gameObject.SetActive(true);
        eventButtonsUI.SetChoices(currentEvent.choiceCodes, HandleClickChoiceButton);
    }

    // 선택지 버튼 클릭 시, 보상 팝업 노출
    private void HandleClickChoiceButton(EventRewardData rewardData)
    {
        eventRewardUI.gameObject.SetActive(true);
        eventRewardUI.SetReward(rewardData, HandleClickConfirm);
    }

    // 보상 팝업 확인 버튼 클릭 시, 팝업을 닫고 EndScript 출력
    private void HandleClickConfirm(int scriptCode, string resultString)
    {
        eventButtonsUI.gameObject.SetActive(false);
        eventRewardUI.gameObject.SetActive(false);
    
        eventScriptUI.StartEndScript(scriptCode, resultString, () => exitButton.gameObject.SetActive(true));
    }
}
