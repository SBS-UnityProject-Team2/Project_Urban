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
        IEnumerable<Deck.DeckCard> receivedDeck = GameManager.Instance.Deck.CardList;
        
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

    private void RenderDeck(IEnumerable<Deck.DeckCard> deckToRender)
    {
        // 1. 초기화
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 슬롯 생성
        foreach (Deck.DeckCard cardInfo in deckToRender)
        {
            GameObject cardObject = Instantiate(cardPrefab, displayArea);
            cardObject.transform.localScale = Vector3.one;

            // 강화 여부에 따라 알맞은 데이터 가져옴
            CardDataEntry cardData = null;

            if (cardInfo.IsEnchanted)
            {
                cardData = CardManager.Instance.GetEnchantedCardData(cardInfo.CardName);
            }

            // 강화 데이터가 없거나 일반 카드라면 기본 데이터 가져오기
            if (cardData == null)
            {
                cardData = CardManager.Instance.GetCardData(cardInfo.CardName);
            }

            if (cardData != null)
            {
                UICard cardScript = cardObject.GetComponent<UICard>();
                cardScript.SetCardDataEntry(cardData);

                OnClickHandler onClick = cardObject.GetComponent<OnClickHandler>();
                onClick.AddClickHandler(() => removeConfirmPopup.OpenPopup(cardData));
            }
        }
    }
}