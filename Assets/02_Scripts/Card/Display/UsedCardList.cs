using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Button))]
public class UsedCardList : MonoBehaviour
{
    [SerializeField] private CardDisplay cardDisplay;
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {        
        cardDisplay.Display("사용한 카드 목록", Battle.Instance.Deck.UsedCardList);
    }
}