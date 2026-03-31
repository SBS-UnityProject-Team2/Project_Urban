using Cysharp.Threading.Tasks;

public class Player : Actor
{
    private void Awake()
    {
        EventBus.AddEventListener(ActorEvent.TurnStart, HandleTurnStart);
        EventBus.AddAsyncEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
        EventBus.AddEventListener(ActorEvent.Dead, HandleDead);
    }

    private void HandleTurnStart(EventPayload eventPayload)
    {
        Battle.Instance.Deck.DrawCard(Battle.Instance.DrawCount);
        Status.Cost.CurCost = Status.Cost.MaxCost + Battle.Instance.ExtraCost;

        EventPayload payload = new()
        {
            eventId = ActorEvent.InitDraw,
            source = this,
            target = this,
        };

        EventBus.Dispatch(payload);
    }

    private async UniTask HandleTurnEnd(EventPayload eventPayload)
    {
        // 방어도 0으로 만들기
        await Battle.Instance.Deck.DiscardAllCard();
    }

    private void HandleDead(EventPayload eventPayload)
    {
        tokenSource.Cancel();
    }
}