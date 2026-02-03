using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Linq;

public class Deck : MonoBehaviour
{   
    [Header("Hand Reference")]
    [SerializeField] private Hand hand;

    private readonly List<Card> unusedCardList = new();     // 뽑을 덱 
    private readonly List<Card> usedCardList = new();       // 사용한 카드 리스트 
    private readonly List<Card> extinctCardList = new();    // 소멸된 카드 리스트
    private readonly List<Card> tempList = new();

    public IEnumerable<Card> UnusedCardList => unusedCardList;
    public IEnumerable<Card> UsedCardList => usedCardList;
    public IEnumerable<Card> ExtinctCardList => extinctCardList;

    public int UnusedCardCount => unusedCardList.Count;
    public int UsedCardCount => usedCardList.Count;
    public int ExtinctCardCount => extinctCardList.Count;
    public int CurrentHandCount => hand.CurHand.Count();

    public Hand Hand => hand;

    public void Init()
    {
        foreach (Card card in DeckManager.Instance.CardList)
        {
            Card copy = Instantiate(card, transform);
            unusedCardList.Add(copy);
        }

        Shuffle();
    }

    public void Draw(int amount = 1)
    {   
        tempList.Clear();

        for (int i = 0; i < amount; i++)
        {
            if (hand.IsHandFull()) 
                break;
            
            if (unusedCardList.Count == 0)
                Shuffle();

            Card drawCard = unusedCardList[^1];

            unusedCardList.RemoveAt(unusedCardList.Count - 1);
            tempList.Add(drawCard);
        }

        hand.AddCards(tempList);
    }

    public void Use(Card card)
    {
        if (card.IsExtinct)
            extinctCardList.Add(card);
        else
            usedCardList.Add(card);

        hand.Remove(card);
    }

    public void Discard(Card card)
    {
        usedCardList.Add(card);
        hand.Remove(card);
    }

    public void DiscardAll()
    {
        foreach (Card card in hand.CurHand)
            usedCardList.Add(card);

        hand.RemoveAll();
    }

    public void Copy(Card card)
    {
        Card copy = Instantiate(card, transform);
        hand.AddCard(copy);
    }


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