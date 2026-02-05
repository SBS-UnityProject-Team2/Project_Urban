using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class EventButtonsUI : MonoBehaviour
{
    [SerializeField] private List<EventButtonUI> buttons = new();
    private readonly UniTaskCompletionSource<EventRewardData> rewardCompletionSource = new();

    public void Init()
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Init();

        gameObject.SetActive(false);
    }

    public void SetChoices(int[] choiceCodes)
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].SetChoice(choiceCodes[i], HandleButtonClick);
    }

    public void HandleButtonClick(EventRewardData rewardData)
    {
        rewardCompletionSource.TrySetResult(rewardData);
    }

    public async UniTask<EventRewardData> GetRewardData()
    {
        return await rewardCompletionSource.Task;
    }
}
