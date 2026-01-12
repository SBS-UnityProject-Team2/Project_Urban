using UnityEngine;
using System.Collections.Generic;

public class Hand : MonoBehaviour
{
    [Header("Hand Settings")]
    [SerializeField] private int maxCardCount = 12;
    [SerializeField] private float spacing = 1.0f;

    [Header("Card Spawn / Despawn Point")]
    [SerializeField] private Transform cardSpawnPoint;          // 카드가 생성될 위치
    [SerializeField] private Transform cardDespawnPoint;        // 카드가 소멸할 위치

    private readonly List<Card> curHand = new();
    public IEnumerable<Card> CurHand => curHand;

    public Card AddCard(CardName cardName)      // void > Card
    {   
        // 내부 함수에서 생성된 카드를 받아서 저장
        Card newCard = InternalAddCard(cardName);
        
        Align();

        // 생성된 카드를 외부로 반환
        return newCard;
    }

    public void AddCards(IEnumerable<CardName> cardNames)
    {
        foreach(CardName cardName in cardNames)
            InternalAddCard(cardName);

        Align();
    }

    public bool RemoveCard(Card card)
    {
        if (!curHand.Contains(card))
            return false;

        curHand.Remove(card);

        DestroyCard(card);
        Align();

        return true;
    }

    public void RemoveAll()
    {
        foreach (Card card in curHand)
            DestroyCard(card);

        curHand.Clear();
    }

    private Card InternalAddCard(CardName cardName)     // void > Card
    {
        Card newCard = CardManager.Instance.CreateCard(cardName, cardSpawnPoint.position, transform);
        curHand.Add(newCard);

        // 생성한 카드를 반환
        return newCard;
    }

    private void DestroyCard(Card card)
    {
        card.MoveTo(cardDespawnPoint.position, () => Destroy(card.gameObject));
    }

    private void Align()
    {
        if (curHand.Count == 0) return;

        float totalWidth = (curHand.Count - 1) * spacing;
        float startX = -totalWidth / 2;

        for (int i = 0; i < curHand.Count; i++)
        {
            // 역순으로 배치 (최신 카드가 왼쪽)
            int reverseIndex = curHand.Count - 1 - i;
            Vector3 cardPos = curHand[reverseIndex].transform.localPosition;
            cardPos.x = startX + (spacing * i);
            cardPos.z = -0.2f * i;
            curHand[reverseIndex].MoveTo(cardPos);
        }
    }
}