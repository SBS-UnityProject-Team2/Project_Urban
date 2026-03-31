using System.Collections.Generic;
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
        InitDraw();
        RegenCost();
    }

    private async UniTask HandleTurnEnd(EventPayload eventPayload)
    {
        Status.Health.Block = 0;

        await Battle.Instance.Deck.DiscardAllCard();
    }

    private void HandleDead(EventPayload eventPayload)
    {
        tokenSource.Cancel();
    }

    private void InitDraw()
    {
        int drawCount = Battle.Instance.DrawCount;
        List<IDrawCountChange> drawCountChanges = Status.EffectList.GetActiveEffectWith<IDrawCountChange>();
        drawCountChanges.ForEach(draw => drawCount += draw.GetDrawCountDelta());

        Battle.Instance.Deck.DrawCard(drawCount);

        InitDrawPayload payload = new()
        {
            source = this,
            target = this,
            drawCount = drawCount
        };
        DispatchEvent(payload);
    }

    private void RegenCost()
    {
        int regenCost = Status.Cost.MaxCost;
        List<ICostRegenChange> costRegenChanges = Status.EffectList.GetActiveEffectWith<ICostRegenChange>();
        costRegenChanges.ForEach(cost => regenCost += cost.GetCostDelta());

        Status.Cost.CurCost = regenCost;
    }   
}