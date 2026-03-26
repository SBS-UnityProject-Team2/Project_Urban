using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[RequireComponent(typeof(Button))]
public class UnusedCardList : MonoBehaviour
{
    [SerializeField] private Transform panelContent;
    //[SerializeField] private Deck deck; // 추후 덱 연결하기

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
        // Deck에서 unusedCardList를 받아오기        
        // List<CardName> nameList = deck.UnusedCardList.Select(card => card.CardData.cardName).ToList();    
    
        // cardDisplay.Display(nameList, panelContent);
        
    }
}