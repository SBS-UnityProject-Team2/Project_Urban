using System.Collections.Generic;
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

    public void RemoveEventListener(ActorEvent actorEvent, UnityAction<ActorEventPayload> handler)
    {
        UnityEvent<ActorEventPayload> unityEvent = TryGetEvent(actorEvent, true);
        unityEvent.RemoveListener(handler);
    }

    public void RemoveEventAllListeners(ActorEvent actorEvent)
    {
        UnityEvent<ActorEventPayload> unityEvent = TryGetEvent(actorEvent, true);
        unityEvent.RemoveAllListeners();
    }

    public void Dispatch(ActorEventPayload actorEventPayload)
    {
        UnityEvent<ActorEventPayload> unityEvent = TryGetEvent(actorEventPayload.eventId, true);
        unityEvent?.Invoke(actorEventPayload);
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