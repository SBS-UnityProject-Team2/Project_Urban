using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExtinctCardList : MonoBehaviour
{   
    [SerializeField] private UICard cardPrefab;
    [SerializeField] private Deck deck; 
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform panelContent;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    public void OnClick()
    {
        panel.SetActive(true);
        CardDisplay.Instance.Display(deck.ExtinctCardList, panelContent, cardPrefab);
    }

    public void OnClose()
    {
        panel.SetActive(false);
    }
}