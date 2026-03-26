using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class Hand : MonoBehaviour
{
    [Header("Hand Settings")]
    [SerializeField] private List<Card> curHand = new();
    [SerializeField] private int maxCardCount = 12;
    [SerializeField] private float spacing = 1.0f;

    [Header("Card Spawn Settings")]
    [SerializeField] private Transform cardSpawnPoint;
    [SerializeField] private Transform cardDespawnPoint;

    public List<Card> CurHand => curHand;

    public async UniTask AddCards(List<Card> cards)
    {
        if (curHand.Count >= maxCardCount)
            return;

        curHand.AddRange(cards);

        foreach (Card card in cards)
        {
            card.transform.position = cardSpawnPoint.position;
            card.gameObject.SetActive(true);
        }

        await Align();
    }

    public async UniTask AddCard(Card card)
    {
        if (curHand.Count >= maxCardCount)
            return;

        curHand.Add(card);
        card.transform.position = cardSpawnPoint.position;
        card.gameObject.SetActive(true);

        await Align();
    }

    public async UniTask RemoveCard(Card card)
    {
        curHand.Remove(card);

        UniTask moveTask = Util.MoveTo(card.gameObject, cardDespawnPoint.position, 0.25f);
        UniTask alignTask = Align();

        await UniTask.WhenAll(moveTask, alignTask);

        card.gameObject.SetActive(false);
    }

    public async UniTask RemoveAllCards()
    {
        List<UniTask> tasks = new();

        foreach (Card card in curHand)
        {
            UniTask moveTask = Util.MoveTo(card.gameObject, cardDespawnPoint.position, 0.25f);
            tasks.Add(moveTask);
        }

        await UniTask.WhenAll(tasks);

        foreach (Card card in curHand)
            card.gameObject.SetActive(false);
        
        curHand.Clear();
    }

    private async UniTask Align()
    {
        if (curHand.Count == 0) return;

        List<UniTask> tasks = new();
        float totalWidth = (curHand.Count - 1) * spacing;
        float startX = -totalWidth / 2;

        for (int i = 0; i < curHand.Count; i++)
        {
            int reverseIndex = curHand.Count - 1 - i;
            Vector3 cardPos = curHand[reverseIndex].transform.localPosition;
            cardPos.x = startX + (spacing * i);
            cardPos.y = 0.0f;
            cardPos.z = -0.2f * i;

            UniTask task = Util.MoveTo(curHand[reverseIndex].gameObject, cardPos, 0.25f);
            tasks.Add(task);
        }

        await UniTask.WhenAll(tasks);
    }
}