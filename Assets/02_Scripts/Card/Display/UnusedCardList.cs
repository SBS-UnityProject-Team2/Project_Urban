using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UnusedCardList : MonoBehaviour
{
    [SerializeField] private CardDisplay cardDisplay;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {        
        cardDisplay.Display("덱 목록", Battle.Instance.Deck.UnusedCardList);
    }
}