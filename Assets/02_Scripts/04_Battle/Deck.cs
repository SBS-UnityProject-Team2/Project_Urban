using UnityEngine;
using System.Collections.Generic;

public class Deck : MonoBehaviour
{
    [SerializeField] private List<Card> unusedCardList = new();     
    [SerializeField] private List<Card> usedCardList = new();       
    [SerializeField] private List<Card> extinctCardList = new();    
    private Hand hand;

    public List<Card> UnusedCardList => unusedCardList;
    public List<Card> UsedCardList => usedCardList;
    public List<Card> ExtinctCardList => extinctCardList;
    public Hand Hand => hand;

    public void Init(List<Card> originCardList, Hand hand)
    {
        unusedCardList.AddRange(originCardList);
        this.hand = hand;
    }

    public void DrawCard(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (unusedCardList.Count == 0)
                ReShuffle();

            Card drawCard = unusedCardList[^1];
            unusedCardList.RemoveAt(unusedCardList.Count - 1);

            hand.AddCard(drawCard);
        }
    }
    
    public void DiscardCard(Card card)
    {
        hand.RemoveCard(card);
        usedCardList.Add(card);
    }

    public void UseCard(Card card)
    {
        hand.RemoveCard(card);

        if (card.CardData.isExtinct)
            extinctCardList.Add(card);
        else
            usedCardList.Add(card);
    }

    public void Shuffle()
    {
        for (int i = unusedCardList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (unusedCardList[i], unusedCardList[j]) = (unusedCardList[j], unusedCardList[i]);
        }
    }

    private void ReShuffle()
    {
        unusedCardList.AddRange(usedCardList);
        usedCardList.Clear();

        Shuffle();
    }
}