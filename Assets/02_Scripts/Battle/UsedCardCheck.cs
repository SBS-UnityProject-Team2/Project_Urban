using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UsedCardCheck : MonoBehaviour
{
    [Header("UI Setting")]
    [SerializeField] private Transform displayArea;       // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;       // 카드 슬롯 프리팹

    private int prevCardCount;

    // 패널 열기
    public void OpenDiscardDisplay()
    {
        // 1. 덱 체크 
        IEnumerable<Deck.DeckCard> usedCards = GameManager.Instance.Deck.UsedCardList;
        
        // 2. 리스트 체크        
        int curUsedCardCount = usedCards.Count();

        // 카드가 변경되었을 때만 다시 그리기
        if (prevCardCount != curUsedCardCount)
        {
            RenderUsedPile(usedCards);
            prevCardCount = curUsedCardCount;
        }

        gameObject.SetActive(true);
    }

    public void CloseDiscardDisplay()
    {
        gameObject.SetActive(false);
    }

    private void RenderUsedPile(IEnumerable<Deck.DeckCard> deckToRender)
    {
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 반복문 변수 타입 변경 (CardName -> Deck.DeckCard)
        foreach (Deck.DeckCard cardInfo in deckToRender)
        {
            GameObject slotObj = Instantiate(cardPrefab, displayArea);
            slotObj.transform.localScale = Vector3.one;

            UICard cardScript = slotObj.GetComponent<UICard>();

            CardDataEntry data = null;

            // 강화 여부 확인
            if (cardInfo.IsEnchanted)
            {
                data = CardManager.Instance.GetEnchantedCardData(cardInfo.CardName);
            }

            // 강화 데이터가 없거나 일반 카드라면 기본 데이터 가져오기
            if (data == null)
            {
                data = CardManager.Instance.GetCardData(cardInfo.CardName);
            }

            if (data != null)
            {
                cardScript.SetCardDataEntry(data);
            }
        }
    }
}