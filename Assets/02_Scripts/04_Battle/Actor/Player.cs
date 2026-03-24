using UnityEngine;

public class Player : Actor
{
    private void Awake()
    {
        EventBus.AddEventListener(ActorEvent.TurnStart, HandleTurnStart);
    }

    private void HandleTurnStart(ActorEventPayload eventPayload)
    {
        Battle.Instance.Deck.DrawCard(Battle.Instance.DrawCount);
    }
}