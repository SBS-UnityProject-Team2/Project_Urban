using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UnusedCardList : MonoBehaviour
{
    [SerializeField] private CardDisplay cardDisplay;
    [SerializeField] private Deck deck; // 추후 덱 연결하기

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
        cardDisplay.Display(deck.UnusedCardList);
        
    }
}