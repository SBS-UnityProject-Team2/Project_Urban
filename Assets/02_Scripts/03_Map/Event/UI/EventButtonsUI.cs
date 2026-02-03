using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventButtonsUI : MonoBehaviour
{
    [SerializeField] private List<EventButtonUI> buttons = new();

    public void Init()
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Init();

        gameObject.SetActive(false);
    }

    public void SetChoices(int[] choiceCodes, UnityAction<EventRewardData> handleClick)
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].SetChoice(choiceCodes[i], handleClick);
    }
}