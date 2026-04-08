using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;
using JetBrains.Annotations;

public class Deck : MonoBehaviour
{
    private readonly CardList unusedCardList = new();
    private readonly CardList usedCardList = new();
    private readonly CardList extinctCardList = new();
    private Hand hand;

    public List<DeckCard> UnusedCardList => unusedCardList.Select(card => card.DeckCard).ToList();
    public List<DeckCard> UsedCardList => usedCardList.Select(card => card.DeckCard).ToList();
    public List<DeckCard> ExtinctCardList => extinctCardList.Select(card => card.DeckCard).ToList();
    public Hand Hand => hand;

    public void Init(List<DeckCard> originDeck, Hand hand)
    {
        foreach (DeckCard cardInstance in originDeck)
            CreateCard(cardInstance, Location.Deck);
        Shuffle();

        this.hand = hand;
    }

    public async UniTask DrawCard()
    {
        await hand.AddCard(InternalDrawCard());
    }

    public async UniTask DrawCard(int amount)
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

        drawCard.transform.parent = hand.transform;
        drawCard.gameObject.SetActive(true);

        return drawCard;
    }

    public async void DiscardCard(Card card)
    {
        await hand.RemoveCard(card);

        usedCardList.Add(card);
        DispatchDiscardCard(card);

        card.gameObject.SetActive(false);
        card.transform.parent = transform;
    }

    public async UniTask DiscardAllCard()
    {
        List<Card> copy = new(hand.CurHand);

        await hand.RemoveAllCards();

        foreach (Card card in copy)
        {
            usedCardList.Add(card);
            DispatchDiscardCard(card);

            card.gameObject.SetActive(false);
            card.transform.parent = transform;
        }
    }

    public async void UseCard(Card card)
    {
        await hand.RemoveCard(card);

        if (card.CardData.isExtinct)
        {
            extinctCardList.Add(card);
            DispatchExtinctCard(card);
        }
        else
        {
            usedCardList.Add(card);
            DispatchDiscardCard(card);
        }

        card.gameObject.SetActive(false);
        card.transform.parent = transform;
    }

    public Card GetCard(Location location, int cardId)
    {
        return location switch
        {
            Location.Deck or Location.DeckBottom or Location.DeckTop => unusedCardList.Find(card => card.Id == cardId),
            Location.DiscardPile => usedCardList.Find(card => card.Id == cardId),
            Location.ExhaustPile => extinctCardList.Find(card => card.Id == cardId),
            Location.Hand => hand.GetCard(cardId),
            _ => null,
        };
    }

    public Card GetCard(int cardId)
    {
        foreach(Location location in System.Enum.GetValues(typeof(Location)))
        {
            return GetCard(location, cardId);
        }

        return null;
    }

    public async void CreateCard(DeckCard deckCard, Location destination)
    {
        Card card = Instantiate(CardManager.Instance.GetCardPrefab(), Vector3.zero, Quaternion.identity, transform);
        card.Init(deckCard);
        card.gameObject.SetActive(false);
        
        switch (destination)
        {
            case Location.Deck:
            case Location.DeckTop:
            case Location.DeckBottom:
                unusedCardList.Add(card);
                break;

            case Location.DiscardPile:
                usedCardList.Add(card);
                break;

            case Location.ExhaustPile:
                extinctCardList.Add(card);
                break;

            case Location.Hand:
                await hand.AddCard(card);
                break;
        }
    }

    public void CreateCard(CardName cardName, Location destination)
    {
        DeckCard cardInstance = new(cardName);
        
        CreateCard(cardInstance, destination);
    }

    public async UniTask MoveCard(Location from, Location to, int amount)
    {
        if (from == to) return;
        List<Card> moveCards = new();

        switch (from)
        {
            case Location.Deck:
            case Location.DeckTop:
                for (int i = 0; i < amount; i++)
                {
                    moveCards.Add(unusedCardList[^1]);
                    unusedCardList.RemoveAt(unusedCardList.Count - 1);
                }
                break;
            case Location.DeckBottom:
                for (int i = 0; i < amount; i++)
                    moveCards.Add(unusedCardList[i]);
                moveCards.RemoveRange(0, amount);
                break;

            case Location.DiscardPile:
                RandomCardMove(usedCardList, moveCards, amount);
                break;

            case Location.ExhaustPile:
                RandomCardMove(extinctCardList, moveCards, amount);
                break;

            case Location.Hand:
                RandomCardMove(hand.CurHand, moveCards, amount);
                break;
        }
        
        switch (to)
        {
            case Location.Deck:
            case Location.DeckTop:
                unusedCardList.AddRange(moveCards);
                break;

            case Location.DeckBottom:
                unusedCardList.InsertRange(0, moveCards);
                break;

            case Location.DiscardPile:
                usedCardList.AddRange(moveCards);
                foreach (Card moveCard in moveCards)
                    DispatchDiscardCard(moveCard);
                break;

            case Location.ExhaustPile:
                extinctCardList.AddRange(moveCards);
                foreach (Card moveCard in moveCards)
                    DispatchExtinctCard(moveCard);
                break;

            case Location.Hand:
                await hand.AddCards(moveCards);
                break;
        }
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

    private void RandomCardMove(List<Card> from, List<Card> to, int amount)
    {
        if (from.Count < amount)
        {
            to.AddRange(usedCardList);
            from.Clear();

            return;
        }

        for (int i = 0; i < amount; i++)
        {
            int randIdx = Random.Range(0, from.Count);
            (from[randIdx], from[^1]) = (from[^1], from[randIdx]);

            to.Add(from[^1]);
            from.RemoveAt(from.Count - 1);
        }

    }

    private void DispatchExtinctCard(Card card)
    {
        ExtinctCardPayload payload = new()
        {
            source = Battle.Instance.Player,
            target = Battle.Instance.Player,
            cardName = card.DeckCard.Name,
        };

        Battle.Instance.Player.DispatchEvent(payload);
    }

    private void DispatchDiscardCard(Card card)
    {
        DiscardCardPayload payload = new()
        {
            source = Battle.Instance.Player,
            target = Battle.Instance.Player,
            cardName = card.DeckCard.Name,
        };

        Battle.Instance.Player.DispatchEvent(payload);
    }
}