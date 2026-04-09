using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;

public enum ActorEvent
{
    None,
    TurnStart,
    TurnEnd,
    InitDraw,
    Draw,
    Action, // 카드 사용
    DiscardCard,
    ExtinctCard,
    Attack,
    Protect,
    GiveEffect,
    RemoveEffect,
    ClearEffect,
    UpdateCurHp,
    UpdateMaxHp,
    UpdateCurCost,
    UpdateMaxCost,
    UpdateElement,
    DamageTaken,
    DamageIncoming ,
    AttackDamageTaken,
    EffectDamageTaken,
    Break,
    Dead,
} 

public class ActorEventBus
{
    readonly private Dictionary<ActorEvent, UnityEvent<EventPayload>> eventMap = new();
    readonly private Dictionary<ActorEvent, List<Func<EventPayload, UniTask>>> asyncEventMap = new();

    public ActorEventBus()
    {
        DamageTakenPayload payload = new();

        AddAsyncEventListener(ActorEvent.AttackDamageTaken, async eventPayload =>
        {
            AttackDamageTakenPayload attackDamageTakenPayload = eventPayload as AttackDamageTakenPayload;

            payload.source = attackDamageTakenPayload.source;
            payload.target = attackDamageTakenPayload.target;
            payload.damage = attackDamageTakenPayload.damage;

            await DispatchAsync(payload);
        });

        AddAsyncEventListener(ActorEvent.EffectDamageTaken, async eventPayload =>
        {
            EffectDamageTakenPayload effectDamageTakenPayload = eventPayload as EffectDamageTakenPayload;

            payload.source = effectDamageTakenPayload.source;
            payload.target = effectDamageTakenPayload.target;
            payload.damage = effectDamageTakenPayload.damage;

            await DispatchAsync(payload);
        });
    }

    private UnityEvent<EventPayload> TryGetEvent(ActorEvent actorEvent, bool createIfMissing)
    {
        if (!eventMap.TryGetValue(actorEvent, out  UnityEvent<EventPayload> unityEvent))
        {
            if (!createIfMissing)
                return null;

            eventMap[actorEvent] = new();

            return  eventMap[actorEvent];
        }

        return unityEvent;
    }

    public void AddEventListener(ActorEvent actorEvent, UnityAction<EventPayload> handler)
    {
        UnityEvent<EventPayload> unityEvent = TryGetEvent(actorEvent, true);
        unityEvent.AddListener(handler);
    }

    public void AddAsyncEventListener(ActorEvent actorEvent, Func<EventPayload, UniTask> handler)
    {
        if (!asyncEventMap.TryGetValue(actorEvent, out List<Func<EventPayload, UniTask>> handlers))
        {
            handlers = new List<Func<EventPayload, UniTask>>();
            asyncEventMap[actorEvent] = handlers;
        }

        handlers.Add(handler);
    }

    public void RemoveEventListener(ActorEvent actorEvent, UnityAction<EventPayload> handler)
    {
        UnityEvent<EventPayload> unityEvent = TryGetEvent(actorEvent, true);
        unityEvent.RemoveListener(handler);
    }

    public void RemoveAsyncEventListener(ActorEvent actorEvent, Func<EventPayload, UniTask> handler)
    {
        if (asyncEventMap.TryGetValue(actorEvent, out List<Func<EventPayload, UniTask>> handlers))
            handlers.Remove(handler);
    }

    public void RemoveEventAllListeners(ActorEvent actorEvent)
    {
        UnityEvent<EventPayload> unityEvent = TryGetEvent(actorEvent, true);
        unityEvent.RemoveAllListeners();

        if (asyncEventMap.TryGetValue(actorEvent, out List<Func<EventPayload, UniTask>> handlers))
            handlers.Clear();
    }

    public void Dispatch(EventPayload EventPayload)
    {
        UnityEvent<EventPayload> unityEvent = TryGetEvent(EventPayload.eventId, true);
        unityEvent?.Invoke(EventPayload);
    }

    public async UniTask DispatchAsync(EventPayload EventPayload)
    {
        Dispatch(EventPayload);

        if (!asyncEventMap.TryGetValue(EventPayload.eventId, out List<Func<EventPayload, UniTask>> handlers))
            return;

        foreach (Func<EventPayload, UniTask> handler in handlers)
            await handler(EventPayload);
    }
}

