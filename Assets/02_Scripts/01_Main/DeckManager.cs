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
        /* 이 방식은 반복문 두번 도는 꼴임....
        CardInstance cardInstance = deck.Find(card => card.InstanceId == instanceId);
        deck.Remove(cardInstance);
        */

        // 정렬 유지를 위해서 그 자리에서 지움
        // 정렬을 하지 않아도 된다면 삭제할 객체를 맨 뒤 인덱스의 객체와 스왑한뒤 삭제한다
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