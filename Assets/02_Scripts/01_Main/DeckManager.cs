using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] private List<CardRecipe> initialDeckRecipe = new();

    private readonly List<DeckCard> deck = new();
    public List<DeckCard> Deck => deck;

    private void Start()
    {
        foreach (CardRecipe cardRecipe in initialDeckRecipe)
        {
            for (int i = 0; i < cardRecipe.count; i++)
                deck.Add(new DeckCard(cardRecipe.name));
        }

        Sort();
    }

    public void Add(CardName cardName)
    {
        deck.Add(new DeckCard(cardName));
        
        Sort();
    }

    public void Remove(int cardId)
    {
        int idx = deck.FindIndex(card => card.Id == cardId);
        deck.RemoveAt(idx);
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
}

[System.Serializable]
public struct CardRecipe
{
    public CardName name;
    public int count;
}