using UnityEngine;
using System.Collections.Generic;

public class Deck 
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

    public Deck(IEnumerable<Card> cards, Hand hand)
    {
        this.hand = hand;

        unusedCardList.AddRange(cards);
        Shuffle();
    }

    // 덱 셔플 로직
    public void Shuffle()
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

    // 카드 여러 장 드로우
    public void Draw(int amount = 1)
    {
        for (int i = 0; i < amount; i++)
        {
            if (hand.IsHandFull()) break;

            if (unusedCardList.Count == 0) Shuffle();
            
            hand.AddCard(GetNextCard());
        }   
    }

    // 다음 카드 가져오기 
    private Card GetNextCard()
    {
        // 리스트의 맨 뒤에서부터 가져옴 
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
        
        // 핸드에서 UI 오브젝트 제거
        hand.RemoveCard(usedCard);
    }

    // 핸드에 있는 모든 카드 버리기
    public void DiscardAll()
    {
        foreach (Card card in hand.CurHand)
            usedCardList.Add(card);

        hand.RemoveAll();
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

    // 뽑을 덱 중간에 카드 찔러 넣기
    public void AddCardToDrawPile(CardName cardName, bool isEnchanted = false)
    {
        Card card = DeckManager.Instance.CreateCard(cardName, isEnchanted);
        
        usedCardList.Add(card);
        Shuffle();
    }

    // 소멸 로직
    public void Extinct(Card card)
    {        
        extinctCardList.Add(card);
        hand.RemoveCard(card);        
    }
    

    // 덱 관리용 카드 데이터 객체
    [System.Serializable]
    public class DeckCard
    {
        public CardName CardName;
        public bool IsEnchanted; // 강화 여부

        public DeckCard(CardName name, bool isEnchanted = false)
        {
            this.CardName = name;
            this.IsEnchanted = isEnchanted;
        }
    }
}