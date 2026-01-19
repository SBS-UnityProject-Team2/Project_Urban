using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DeckDisplayPanel : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform displayArea;       // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;       // 카드 슬롯 프리팹

    private int prevCardCount;

    public void OpenDeckDisplay()
    {
        // 1. GameManager 체크
        if (GameManager.Instance == null)
        {
            Debug.LogError("DeckDisplayPanel: GameManager.Instance가 없습니다!");
            return;
        }

        // 2. 덱 체크 
        if (GameManager.Instance.Deck == null)
        {
            Debug.LogError("DeckDisplayPanel: GameManager.Instance.Deck이 초기화되지 않았습니다!");
            return;
        }
        
        IEnumerable<Deck.DeckCard> receivedDeck = GameManager.Instance.Deck.CardList;
        
        // 3. 카드 리스트 자체 체크
        if (receivedDeck == null)
        {
            Debug.LogError("DeckDisplayPanel: 카드 리스트(CardList)가 Null입니다!");
            return;
        }

        int curCardCount = receivedDeck.Count();

        // 카드가 변경되었을 때만 다시 그리기 
        if (prevCardCount != curCardCount)
        {
            RenderDeck(receivedDeck);
            prevCardCount = curCardCount;
        }

        gameObject.SetActive(true);
    }

    public void CloseDeckDisplay()
    {
        gameObject.SetActive(false);
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
            GameObject slotObj = Instantiate(cardPrefab, displayArea);
            slotObj.transform.localScale = Vector3.one;

            UICard cardScript = slotObj.GetComponent<UICard>();
            CardDataEntry data = null;

            // 강화된 카드라면 강화 데이터를 우선 검색
            if (cardInfo.IsEnchanted)
            {
                data = CardManager.Instance.GetEnchantedCardData(cardInfo.CardName);
            }
            if (data == null)
            {
                data = CardManager.Instance.GetCardData(cardInfo.CardName);
            }

            // 데이터 세팅
            if (data != null)
            {
                cardScript.SetCardDataEntry(data);
            }
        }
    }
}