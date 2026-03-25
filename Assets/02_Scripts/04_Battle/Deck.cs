using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class Deck : MonoBehaviour
{
    [Header("Prefab Setting")]
    [SerializeField] private Card cardPrefab;

    private readonly List<Card> unusedCardList = new();
    private readonly List<Card> usedCardList = new();
    private readonly List<Card> extinctCardList = new();
    private Hand hand;

    public List<Card> UnusedCardList => unusedCardList;
    public List<Card> UsedCardList => usedCardList;
    public List<Card> ExtinctCardList => extinctCardList;
    public Hand Hand => hand;

    public void Init(List<CardInstance> originDeck, Hand hand)
    {
        foreach (CardInstance cardInstance in originDeck)
        {
            Card card = cardInstance.Instantiate(cardPrefab, Vector3.zero, hand.transform);
            card.gameObject.SetActive(false);

            unusedCardList.Add(card);
        }

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

        Card drawCard = unusedCardList[^1];
        unusedCardList.RemoveAt(unusedCardList.Count - 1);

        return drawCard;
    }

    public async void DiscardCard(Card card)
    {
        usedCardList.Add(card);
        await hand.RemoveCard(card);
    }

    public async UniTask DiscardAllCard()
    {
        foreach (Card card in hand.CurHand)
            usedCardList.Add(card);

        await hand.RemoveAllCards();
    }

    public async void UseCard(Card card)
    {
        await hand.RemoveCard(card);

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