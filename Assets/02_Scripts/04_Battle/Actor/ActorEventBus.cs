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
    Action,
    Attack,
    Protect,
    Buff,
    Debuff,
    DamageTaken,
    AttackDamageTaken,
    EffectDamageTaken,
    Break,
    Dead,
} 

public class ActorEventBus
{
    readonly private Dictionary<ActorEvent, UnityEvent<ActorEventPayload>> eventMap = new();
    readonly private Dictionary<ActorEvent, List<Func<ActorEventPayload, UniTask>>> asyncEventMap = new();

    private UnityEvent<ActorEventPayload> TryGetEvent(ActorEvent actorEvent, bool createIfMissing)
    {
        if (!eventMap.TryGetValue(actorEvent, out  UnityEvent<ActorEventPayload> unityEvent))
        {
            if (!createIfMissing)
                return null;

            eventMap[actorEvent] = new();

            return  eventMap[actorEvent];
        }

        return unityEvent;
    }

    public void AddEventListener(ActorEvent actorEvent, UnityAction<ActorEventPayload> handler)
    {
        UnityEvent<ActorEventPayload> unityEvent = TryGetEvent(actorEvent, true);
        unityEvent.AddListener(handler);
    }

    public void AddAsyncEventListener(ActorEvent actorEvent, Func<ActorEventPayload, UniTask> handler)
    {
        if (!asyncEventMap.TryGetValue(actorEvent, out List<Func<ActorEventPayload, UniTask>> handlers))
        {
            handlers = new List<Func<ActorEventPayload, UniTask>>();
            asyncEventMap[actorEvent] = handlers;
        }

        handlers.Add(handler);
    }

    public void RemoveEventListener(ActorEvent actorEvent, UnityAction<ActorEventPayload> handler)
    {
        UnityEvent<ActorEventPayload> unityEvent = TryGetEvent(actorEvent, true);
        unityEvent.RemoveListener(handler);
    }

    public void RemoveAsyncEventListener(ActorEvent actorEvent, Func<ActorEventPayload, UniTask> handler)
    {
        if (asyncEventMap.TryGetValue(actorEvent, out List<Func<ActorEventPayload, UniTask>> handlers))
            handlers.Remove(handler);
    }

    public void RemoveEventAllListeners(ActorEvent actorEvent)
    {
        UnityEvent<ActorEventPayload> unityEvent = TryGetEvent(actorEvent, true);
        unityEvent.RemoveAllListeners();

        if (asyncEventMap.TryGetValue(actorEvent, out List<Func<ActorEventPayload, UniTask>> handlers))
            handlers.Clear();
    }

    public void Dispatch(ActorEventPayload actorEventPayload)
    {
        UnityEvent<ActorEventPayload> unityEvent = TryGetEvent(actorEventPayload.eventId, true);
        unityEvent?.Invoke(actorEventPayload);
    }

    public async UniTask DispatchAsync(ActorEventPayload actorEventPayload)
    {
        Dispatch(actorEventPayload);

        if (!asyncEventMap.TryGetValue(actorEventPayload.eventId, out List<Func<ActorEventPayload, UniTask>> handlers))
            return;

        foreach (Func<ActorEventPayload, UniTask> handler in handlers)
            await handler(actorEventPayload);
    }
}

public class ActorEventPayload
{
    public ActorEvent eventId;
    public Actor source;
    public Actor target;
    public int damage;
    public int healPoint;


    public void Init()
    {
        eventId = ActorEvent.None;
    }
}