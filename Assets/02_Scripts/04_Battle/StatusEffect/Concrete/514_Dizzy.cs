public class Dizzy : DurationEffect
{
    private readonly int additionalCost = 1;
    public override StatusEffectName Name => StatusEffectName.Dizzy;

    public Dizzy(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public override void GiveDuration(int duration = 1)
    {
        if (!isActive)
        {
            ActionPayload payload = new()
            {
                actionId = ActorAction.AddCardCost,
                source = owner,
            };

            foreach (Card card in Battle.Instance.Deck.Hand.CurHand)
            {
                payload.Init();
                payload.actionId = ActorAction.AddCardCost;
                payload.source = owner;
                payload.Write(card.Id);
                payload.Write(additionalCost);
                ActionBus.Dispatch(payload);
            }
        }

        base.GiveDuration(duration);
    }

    public override void Clear()
    {
        base.Clear();

        ActionPayload payload = new()
        {
            actionId = ActorAction.ResetCardCost,
            source = owner,
        };

        foreach (Card card in Battle.Instance.Deck.Hand.CurHand)
        {
            payload.Init();
            payload.actionId = ActorAction.ResetCardCost;
            payload.source = owner;
            payload.Write(card.Id);
            ActionBus.Dispatch(payload);
        }

    }
    
    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}