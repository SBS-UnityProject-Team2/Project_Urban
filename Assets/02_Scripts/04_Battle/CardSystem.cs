using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CardSystem : MonoBehaviour
{
    [Header("GameObject Reference")]
    [SerializeField] private Deck deck;
    [SerializeField] private Hand hand;

    [Header("UI Reference")]
    [SerializeField] private DiscardPanelUI discardPanelUI;

    private readonly List<Card> tempList = new();

    public Deck Deck => deck;
    public Hand Hand => hand;
    public DiscardPanelUI DiscardPanelUI => discardPanelUI;

    public void Init()
    {
        deck.Init();
    }

    public bool Draw()
    {
        if (hand.IsHandFull()) 
            return false;

        hand.AddCard(deck.GetNextCard());
        
        return true;
    }

    public bool Draw(int amount = 1)
    {
        if (hand.IsHandFull()) 
            return false;

        amount = Math.Min(amount, hand.CurHandLeftCount);
        tempList.Clear();
        
        for (int i = 0; i < amount; i++)
            tempList.Add(deck.GetNextCard());       

        hand.AddCards(tempList);

        return true;
    }

    public void Discard(Card card)
    {
        hand.RemoveCard(card);
        deck.Discard(card);
    }

    public void DiscardAll()
    {
        deck.DiscardAll();
        hand.RemoveAll();
    }

    public void CopyCardToDeck(Card card)
    {
        Card copy = Instantiate(card, deck.transform);
        deck.AddCard(copy);
    }

    public void AddDiscardCard(Card card)
    {
        discardPanelUI.AddCard(card);
    }

    public void OpenDiscardPanel(int minCount, int maxCount, UnityAction<int> onComplete = null)  
    {
        discardPanelUI.OpenPanel(minCount, maxCount);

        discardPanelUI.OnConfirm.AddListener(discardList =>
        {
            foreach (Card card in discardList)
                Discard(card);

            onComplete?.Invoke(discardList.Count);
            discardPanelUI.ClosePanel();
        });
    }
}