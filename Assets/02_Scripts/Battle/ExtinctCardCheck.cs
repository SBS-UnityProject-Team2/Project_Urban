using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class ExtinctCardCheck : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform displayArea;       // ScrollView의 Content
    [SerializeField] private GameObject cardPrefab;       // 카드 슬롯 프리팹 

    private int prevCardCount = -1; // 초기값 -1로 설정하여 처음에 무조건 갱신

    public void OpenExtinctDisplay()
    {
        if (GameManager.Instance == null || GameManager.Instance.Deck == null) return;
        
        IEnumerable<Deck.DeckCard> extinctCards = GameManager.Instance.Deck.ExtinctCardList;

        if (extinctCards == null)
        {
            extinctCards = new List<Deck.DeckCard>();
        }

        int curextinctcardCount = extinctCards.Count();

        if (prevCardCount != curextinctcardCount)
        {
            RenderExtinctPile(extinctCards);
            prevCardCount = curextinctcardCount;
        }

        gameObject.SetActive(true);
    }

    public void CloseExtinctDisplay()
    {
        gameObject.SetActive(false);
    }

    private void RenderExtinctPile(IEnumerable<Deck.DeckCard> deckToRender)
    {
        // 1. 초기화
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 소멸카드 슬롯 생성
        foreach (Deck.DeckCard cardInfo in deckToRender)
        {
            GameObject slotObj = Instantiate(cardPrefab, displayArea);
            slotObj.transform.localScale = Vector3.one;

            UICard cardScript = slotObj.GetComponent<UICard>();

            if (cardScript != null)
            {
                // 강화 여부에 따라 데이터 가져오는 로직 추가
                CardDataEntry data = null;

                // 소멸된 카드가 강화된 상태였다면 강화 데이터 검색
                if (cardInfo.IsEnchanted)
                {
                    data = CardManager.Instance.GetEnchantedCardData(cardInfo.CardName);
                }

                // 강화 데이터가 없거나 일반 카드라면 기본 데이터 검색
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
}