using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventButtonsUI : MonoBehaviour
{
    [SerializeField] private List<EventButtonUI> buttons = new(); 

    private EventChoice selectedChoice;

    public void Init(int [] choiceCodes, UnityAction<int> onSelectHandler)
    {
        for (int i = 0; i < choiceCodes.Length; i++)
            buttons[i].Init(choiceCodes[i]);
    }
}