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

    public bool IsHandFull()
    {
        return curHand.Count >= maxCardCount;
    }

    public void AddCard(Card card)     
    {   
        curHand.Add(card);
        card.transform.parent = transform;
        
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