using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    private readonly Hand hand;

    // 이제 덱에서는 이름 + 강화 여부가 포함된 객체 목록으로 관리
    private readonly List<Card> unusedCardList = new();     // 뽑을 덱 
    private readonly List<Card> usedCardList = new();       // 사용한 카드 리스트 
    private readonly List<Card> extinctCardList = new();    // 소멸된 카드 리스트

    // Property
    public Hand Hand => hand;

    public IEnumerable<Card> UnusedCardList => unusedCardList;
    public IEnumerable<Card> UsedCardList => usedCardList;
    public IEnumerable<Card> ExtinctCardList => extinctCardList;

    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;
    public int ExtinctCardCount => extinctCardList.Count;

    public void Init()
    {
        foreach (Card card in DeckManager.Instance.CardList)
        {
            Card copy = Instantiate(card, transform);
            unusedCardList.Add(copy);
        }

        Shuffle();
    }
    
    public void AddCard(Card card)
    {
        unusedCardList.Add(card);
        
        Shuffle();
    }

    public Card GetNextCard()
    {
        if (unusedCardList.Count == 0) 
            Shuffle();

        Card card = unusedCardList[^1];
        unusedCardList.RemoveAt(unusedCardList.Count - 1);
            
        return card; 
    }

    // 사용한 카드 버리기
    public void Discard(Card usedCard)
    {
        // 1. 소멸(Extinct) 체크
        if (usedCard.IsExtinct)
            extinctCardList.Add(usedCard);
        else
            usedCardList.Add(usedCard);
    }

    // 핸드에 있는 모든 카드 버리기
    public void DiscardAll()
    {
        foreach (Card card in hand.CurHand)
            usedCardList.Add(card);
    }

    // 버린 카드 더미에서 랜덤 뽑기 
    public bool DrawRandomFromDiscard(out Card card)
    {       
        if (usedCardList.Count == 0 || hand.IsHandFull())
        {
            card = null;

            return false;
        }

        int randomIndex = Random.Range(0, usedCardList.Count);
        card = usedCardList[randomIndex];

        (usedCardList[randomIndex], usedCardList[^1]) = (usedCardList[^1], usedCardList[randomIndex]);
        usedCardList.RemoveAt(usedCardList.Count - 1);
        hand.AddCard(card);

        return true;
    }

    // 가장 최근에 사용한 카드 정보 확인
    public Card GetLastUsedCard()
    {
        if (usedCardList.Count > 0)
        {
            return usedCardList[^1];
        }
        return null; 
    }

    private void Shuffle()
    {
        // 버린 카드 더미가 있다면 뽑을 덱으로 합침
        if (usedCardList.Count > 0)
        {
            unusedCardList.AddRange(usedCardList);
            usedCardList.Clear();
        }

        // Fisher-Yates 알고리즘으로 섞기
        for (int i = unusedCardList.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            // 튜플 분해를 이용한 스왑
            (unusedCardList[rand], unusedCardList[i]) = (unusedCardList[i], unusedCardList[rand]);
        }
    }
}