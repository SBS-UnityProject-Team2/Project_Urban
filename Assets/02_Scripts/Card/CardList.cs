using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class CardList : List<Card>
{
    // 카드 제거
    public UnityEvent OnRemove = new();

    // 카드 추가
    public UnityEvent OnAdd = new();

    public new void Add(Card card)
    {
        base.Add(card);
        OnAdd.Invoke();
    }

    public new bool Remove(Card card)
    {
        bool removed = base.Remove(card);

        if (removed)
            OnRemove.Invoke();

        return removed;
    }

    public new void RemoveAt(int index)
    {
        base.RemoveAt(index);
        OnRemove.Invoke();
    }

    public new void AddRange(IEnumerable<Card> collection)
    {
        int beforeCount = Count;
        base.AddRange(collection);

        OnAdd.Invoke();
    }

    public new void InsertRange(int index, IEnumerable<Card> collection)
    {
        int beforeCount = Count;
        base.InsertRange(index, collection);

        OnAdd.Invoke();
    }

    public void Select(Action<Card> action)
    {
        foreach (Card card in this)
            action(card);
    }

    public new void Clear()
    {
        if (Count == 0)
            return;

        base.Clear();
        OnRemove.Invoke();
    }
}