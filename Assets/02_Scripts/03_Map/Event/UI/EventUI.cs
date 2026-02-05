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
        exitButton.onClick.AddListener(ButtonEvent.Instance.OnClickEventExit);    
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
        await PlayEventScript();

        EventRewardData rewardData = await PlayEventChoice();
        
        (int scriptCode, string resultString) = await PlayEventReward(rewardData);
        
        await PlayEndScript(scriptCode, resultString);

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
}
