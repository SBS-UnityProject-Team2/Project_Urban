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
        IEnumerable<Card> extinctCards = GameManager.Instance.Deck.ExtinctCardList;
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

    private void RenderExtinctPile(IEnumerable<Card> deckToRender)
    {
        // 1. 초기화
        foreach (Transform child in displayArea)
        {
            Destroy(child.gameObject);
        }

        // 2. 소멸카드 슬롯 생성
        foreach (Card cardInfo in deckToRender)
        {
            GameObject slotObj = Instantiate(cardPrefab, displayArea);
            slotObj.transform.localScale = Vector3.one;

            UICard cardScript = slotObj.GetComponent<UICard>();
            cardScript.SetCardDataEntry(cardInfo.Data);
        }
    }
}