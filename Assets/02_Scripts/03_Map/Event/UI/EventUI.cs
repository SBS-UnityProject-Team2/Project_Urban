using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

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
        exitButton.onClick.AddListener(HandleExitButtonClick);
    }

    private void OnDestroy()
    {
        exitButton.onClick.RemoveListener(HandleExitButtonClick);
    }

    private void OnEnable()
    {
        eventScriptUI.Init();
        eventButtonsUI.Init();
        eventRewardUI.Init();
        exitButton.gameObject.SetActive(false);

        currentEvent = EventManager.Instance.GetRandomEvent();

        PlayEvent();
    }

    private async void PlayEvent()
    {
        // 스크립트 출력이 끝날때까지 대기
        await PlayEventScript();

        // 유저가 선택지를 고를때까지 대기
        EventRewardData rewardData = await PlayEventChoice();
        
        eventButtonsUI.gameObject.SetActive(false);
        
        // 유저가 보상을 받을때까지 대기
        (int scriptCode, string resultString) = await PlayEventReward(rewardData);
        
        // 스크립트 출력이 끝날때까지 대기
        await PlayEndScript(scriptCode, resultString);

        // 나가기 버튼 활성화
        exitButton.gameObject.SetActive(true);
    }

    private async UniTask PlayEventScript()
    {
        eventScriptUI.gameObject.SetActive(true);
        await eventScriptUI.StartEventScript(currentEvent.scriptCode);
    }

    private async UniTask<EventRewardData> PlayEventChoice()
    {
        eventButtonsUI.gameObject.SetActive(true);
        eventButtonsUI.SetChoices(currentEvent.choiceCodes);
        
        return await eventButtonsUI.GetRewardData();
    }

    private async UniTask<(int, string)> PlayEventReward(EventRewardData rewardData)
    {
        eventRewardUI.gameObject.SetActive(true);
        eventRewardUI.SetReward(rewardData);
        return await eventRewardUI.GetResult();
    }

    private async UniTask PlayEndScript(int scriptCode, string resultString)
    {
        eventButtonsUI.gameObject.SetActive(false);
        eventRewardUI.gameObject.SetActive(false);

        await eventScriptUI.StartEndScript(scriptCode, resultString);
    }

    private void HandleExitButtonClick()
    {
        if (ButtonEvent.Instance != null)
            ButtonEvent.Instance.OnClickEventExit();
    }
}
