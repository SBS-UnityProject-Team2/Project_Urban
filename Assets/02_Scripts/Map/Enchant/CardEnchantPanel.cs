using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CardEnchantPanel : MonoBehaviour
{

    [Header("Display Settings")]
    [SerializeField] private Transform displayArea;   // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;   // 카드 슬롯 프리팹
    [SerializeField] private GameObject EnchantCardPanel; // 인챈트 패널

    [Header("Popup Settings")]
    [SerializeField] private EnchantConfirmPopup deckEnchantPopup;

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

        EnchantCardPanel.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        EnchantCardPanel.SetActive(false);
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

            // 강화 여부에 따라 알맞은 데이터 불러옴
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

                // 클릭 시 팝업 열기
                OnClickHandler onClick = cardObject.GetComponent<OnClickHandler>();
                onClick.AddClickHandler(() => deckEnchantPopup.OpenPopup(cardData));
            }
        }
    }
}