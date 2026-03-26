using System.Collections.Generic;
using UnityEngine;

public class DeckManager : Singleton<DeckManager>
{
    [SerializeField] private List<CardRecipe> initialDeckRecipe = new();

    private readonly List<CardInstance> deck = new();
    public List<CardInstance> Deck => deck;

    private void Start()
    {
        foreach (CardRecipe cardRecipe in initialDeckRecipe)
        {
            for (int i = 0; i < cardRecipe.count; i++)
                deck.Add(new CardInstance(cardRecipe.name));
        }
    }

    public void Add(CardName cardName)
    {
        deck.Add(new CardInstance(cardName));
    }

    public void Remove(int instanceId)
    {
        /* 이 방식은 반복문 두번 도는 꼴임....
        CardInstance cardInstance = deck.Find(card => card.InstanceId == instanceId);
        deck.Remove(cardInstance);
        */

        int idx = deck.FindIndex(card => card.InstanceId == instanceId);

        (deck[idx], deck[^1]) = (deck[^1], deck[idx]);
        deck.RemoveAt(deck.Count - 1);
    }

    public void Enchant(int instanceId)
    {
        CardInstance cardInstance = deck.Find(card => card.InstanceId == instanceId);
        cardInstance.Enchant();
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