using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardRemovePanel : MonoBehaviour
{
    [Header("Display Settings")]
    [SerializeField] private Transform displayArea;       // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;   // 카드 슬롯 프리팹
    [SerializeField] private GameObject RemoveCardPanel; // 패널
    [Header("Popup Settings")]
    [SerializeField] private RemoveConfirmPopup removeConfirmPopup;

    private int prevCardCount;

    public void OpenDeckDisplay()
    {
        IEnumerable<CardName> receivedDeck = GameManager.Instance.Deck.CardList;
        int curCardCount = receivedDeck.Count();

        if (prevCardCount != curCardCount)
        {
            RenderDeck(receivedDeck);
            prevCardCount = curCardCount;
        }

        RemoveCardPanel.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        RemoveCardPanel.SetActive(false);
    }

    private void RenderDeck(IEnumerable<CardName> deckToRender)
    {
        // 1. 초기화
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 슬롯 생성
        foreach (CardName cardName in deckToRender)
        {
            GameObject cardObject = Instantiate(cardPrefab, displayArea);
            cardObject.transform.localScale = Vector3.one;

            CardDataEntry cardData = CardManager.Instance.GetCardData(cardName);

            UICard cardScript = cardObject.GetComponent<UICard>();
            cardScript.SetCardDataEntry(cardData);

            OnClickHandler onClick = cardObject.GetComponent<OnClickHandler>();
            onClick.AddClickHandler(() => removeConfirmPopup.OpenPopup(cardData));
        }
    }
}