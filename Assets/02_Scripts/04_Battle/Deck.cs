using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class Deck : MonoBehaviour
{
    private readonly List<CardInstance> unusedCardList = new();
    private readonly List<CardInstance> usedCardList = new();
    private readonly List<CardInstance> extinctCardList = new();
    private Hand hand;

    public List<CardInstance> UnusedCardList => unusedCardList;
    public List<CardInstance> UsedCardList => usedCardList;
    public List<CardInstance> ExtinctCardList => extinctCardList;
    public Hand Hand => hand;

    public void Init(List<CardInstance> originDeck, Hand hand)
    {
        unusedCardList.AddRange(originDeck);
        this.hand = hand;
    }

    public async void DrawCard()
    {
        await hand.AddCard(InternalDrawCard());
    }

    public async void DrawCard(int amount)
    {
        List<Card> cards = new();
        for (int i = 0; i < amount; i++)
            cards.Add(InternalDrawCard());

        await hand.AddCards(cards);
    }

    private Card InternalDrawCard()
    {
        if (unusedCardList.Count == 0)
            ReShuffle();

        CardInstance drawCard = unusedCardList[^1];
        unusedCardList.RemoveAt(unusedCardList.Count - 1);

        return drawCard.Instantiate(CardManager.Instance.GetCardPrefab(), Vector3.zero, hand.transform);
    }

    public async void DiscardCard(Card card)
    {
        await hand.RemoveCard(card);

        usedCardList.Add(card.CardInstance);
        Destroy(card.gameObject);
    }

    public async UniTask DiscardAllCard()
    {
        List<Card> copy = new(hand.CurHand);

        await hand.RemoveAllCards();

        foreach (Card card in copy)
        {
            usedCardList.Add(card.CardInstance);
            Destroy(card.gameObject);
        }
    }

    public async void UseCard(Card card)
    {
        await hand.RemoveCard(card);

        if (card.CardData.isExtinct)
            extinctCardList.Add(card.CardInstance);
        else
            usedCardList.Add(card.CardInstance);
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