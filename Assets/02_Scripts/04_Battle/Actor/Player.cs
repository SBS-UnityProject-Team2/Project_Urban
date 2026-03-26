using Cysharp.Threading.Tasks;

public class Player : Actor
{
    private void Awake()
    {
        EventBus.AddEventListener(ActorEvent.TurnStart, HandleTurnStart);
        EventBus.AddAsyncEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    private void HandleTurnStart(ActorEventPayload eventPayload)
    {
        Battle.Instance.Deck.DrawCard(Battle.Instance.DrawCount);
    }

    private async UniTask HandleTurnEnd(ActorEventPayload eventPayload)
    {
        await Battle.Instance.Deck.DiscardAllCard();
    }

    private void HandleDead(ActorEventPayload eventPayload)
    {
        tokenSource.Cancel();
    }
}