using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] private List<CardRecipe> initialDeckRecipe = new();
    [SerializeField] private List<DeckCard> deck = new();

    public ElementType SelectedElement { get; set; } = ElementType.None;
    public List<DeckCard> Deck => deck;
    public List<DeckCard> CardList => deck;

    private void Start()
    {
        foreach (CardRecipe cardRecipe in initialDeckRecipe)
        {
            for (int i = 0; i < cardRecipe.count; i++)
                deck.Add(new DeckCard(cardRecipe.name));
        }

        Sort();
    }

    public void AddCard(CardName cardName)
    {
        deck.Add(new DeckCard(cardName));
        Sort();
    }

    public void AddCards(List<CardName> cardNames)
    {
        foreach(CardName name in cardNames)
            deck.Add(new DeckCard(name));

        Sort();
    }

    public void Remove(int cardId)
    {
        int idx = deck.FindIndex(card => card.Id == cardId);
        deck.RemoveAt(idx);
    }

    public void RemoveCard(DeckCard card)
    {
        if (card == null)
            return;

        Remove(card.Id);
    }

    public void Enchant(int cardId)
    {
        DeckCard card = deck.Find(card => card.Id == cardId);
        card.Enchant();
    }

    public void Sort()
    {
        deck.Sort((card1, card2) => card1.Name.CompareTo(card2.Name));
    }

    public DeckCard GetRandomCard(ElementType element)
    {       

        if (element == ElementType.None)
            return deck[Random.Range(0, deck.Count)];

        List<DeckCard> filteredDeck = deck
            .Where(card => Util.GetElement(card.Name) == element)
            .ToList();       

        return filteredDeck[Random.Range(0, filteredDeck.Count)];
    }
}

[System.Serializable]
public struct CardRecipe
{
    public CardName name;
    public int count;
}