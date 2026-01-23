using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CardRecipe
{
    public CardName name;
    public int count;
}

public class DeckManager : Singleton<DeckManager>
{
    [Header("Initial Deck Recipe")]
    [SerializeField] private List<CardRecipe> initialDeckRecipe = new();
    private readonly List<Card> cardList = new();
    public IEnumerable<Card> CardList => cardList;

    private void Start()
    {
        foreach (CardRecipe recipe in initialDeckRecipe)
        {
            for (int i = 0; i < recipe.count; i++)
                InternalAddCard(recipe.name);

            SortCardList();
        }
    }

    public Card CreateCard(CardName cardName, bool isEnchanted = false)
    {
        return CardManager.Instance.CreateCard
        (
            cardName,
            transform,
            isEnchanted
        );
    }

    public void AddCards(IEnumerable<CardName> cardNames)
    {
        foreach (CardName cardName in cardNames)
            InternalAddCard(cardName);

        SortCardList();
    }

    public void AddCard(CardName cardName)
    {
        InternalAddCard(cardName);
        SortCardList();
    }

    private void InternalAddCard(CardName cardName)
    {
        Card card = CreateCard(cardName, false);
        cardList.Add(card);
    }

    public void RemoveCard(Card card)
    {
        cardList.Remove(card);
    }

    private void SortCardList()
    {
        cardList.Sort(CardListSortCompare);
    }

    public int CardListSortCompare(Card card1, Card card2)
    {
        return ((int)card1.Name).CompareTo((int)card2.Name);
    }
}