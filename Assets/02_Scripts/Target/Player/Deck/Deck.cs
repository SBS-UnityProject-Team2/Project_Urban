using UnityEngine;
using System.Collections.Generic;

public class Deck 
{
    private readonly List<CardName> originCardList = new();     // 원본 덱 
    private readonly List<CardName> unusedCardList = new();     // 뽑을 덱 
    private readonly List<CardName> usedCardList = new();       // 사용한 카드리스트
    private readonly List<CardName> tempCardList = new(12);         // 카드 임시 버퍼

    private Hand hand;
    
    public IEnumerable<CardName> CardList => originCardList;
    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;

    public Deck(IEnumerable<CardName> cardRecipes)
    {
        foreach (CardName name in cardRecipes)
        {
            originCardList.Add(name);
            unusedCardList.Add(name);
        }

        Shuffle();
    }
    
    public void Initialize(Hand hand)
    {
        this.hand = hand;
    }


    public void Shuffle()
    {
        if (usedCardList.Count > 0)
        {
            unusedCardList.AddRange(usedCardList);
            usedCardList.Clear();
        }

        for (int i = unusedCardList.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            (unusedCardList[rand], unusedCardList[i]) = (unusedCardList[i], unusedCardList[rand]);
        }
    }

    public void Draw(int amount)
    {
        tempCardList.Clear();
        
        for (int i = 0; i < amount; i++)
            tempCardList.Add(GetNextCardName());

        hand.AddCards(tempCardList);
    }

    public void Draw()
    {
        hand.AddCard(GetNextCardName());
    }

    private CardName GetNextCardName()
    {
        if (unusedCardList.Count == 0)
            Shuffle();

        CardName cardName = unusedCardList[^1];
        unusedCardList.RemoveAt(unusedCardList.Count - 1);

        return cardName;
    }

    // 사용한 카드 UsedCardList 로 보내기
    public void Discard(Card usedCard)
    {
        usedCardList.Add(usedCard.Name);
        hand.RemoveCard(usedCard);
    }

    public void DiscardAll()
    {
        foreach (Card card in hand.CurHand)
        {
            usedCardList.Add(card.Name);
        }

        hand.RemoveAll();
    }

    public void ResetDeck()
    {
        unusedCardList.Clear();
        usedCardList.Clear();

        foreach (var cardName in originCardList)
            unusedCardList.Add(cardName);

        Shuffle();
    }

    // 카드 추가
    public void AddCard(CardName cardName)
    {
        originCardList.Add(cardName);
    }

    // 카드 제거
    public void RemoveCard(CardName cardName)
    {
        if (originCardList.Contains(cardName))
            originCardList.Remove(cardName);
    }
    
    // 현재 덱에있는 모든 카드 목록 전달용
    public List<CardName> GetAllCards()
    {
        return new List<CardName>(originCardList);
    }

    //UsedCardList 에서 1장 랜덤으로 뽑기로직
    public Card DrawRandomFromDiscard()
    {       

        // 1. 무작위 인덱스 선택
        int randomIndex = Random.Range(0, usedCardList.Count);
        CardName targetCardName = usedCardList[randomIndex];

        // 2. 리스트에서 제거
        usedCardList.RemoveAt(randomIndex);

        // 3. 손패에 추가하고, 추가된 카드 객체를 반환
        // Hand 스크립트의 AddCard 함수가 Card를 반환하도록 수정
        return hand.AddCard(targetCardName);
    }

    
}