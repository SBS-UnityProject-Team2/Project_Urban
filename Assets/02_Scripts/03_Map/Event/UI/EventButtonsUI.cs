using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventButtonsUI : MonoBehaviour
{
    [SerializeField] private List<EventButtonUI> buttons = new(); 

    public void Init(params int [] choiceCodes)
    {
        for (int i = 0; i < buttons.Count; i++)
            buttons[i].Init(choiceCodes[i]);
    }
}