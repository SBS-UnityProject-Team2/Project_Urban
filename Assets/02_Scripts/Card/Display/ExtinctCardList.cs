using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ExtinctCardList : MonoBehaviour
{   
    [SerializeField] private CardDisplay cardDisplay;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {        
        cardDisplay.Display("소멸한 카드 목록", Battle.Instance.Deck.ExtinctCardList);
    }
}