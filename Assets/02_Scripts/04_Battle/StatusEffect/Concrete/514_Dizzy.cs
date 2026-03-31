public class Dizzy : DurationEffect
{
    private readonly int additionalCost = 1;
    public override StatusEffectName Name => StatusEffectName.Dizzy;

    public Dizzy(Actor owner) : base(owner) { }

    public override void GiveDuration(int duration = 1)
    {
        if (!isActive)
        {
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);

            AddCardCostPayload payload = new()
            {
                source = owner,
                costPoint = additionalCost,
            };

            foreach (Card card in Battle.Instance.Deck.Hand.CurHand)
            {
                payload.cardInstanceId = card.Id;
                ActionBus.Dispatch(payload);
            }
        }

        base.GiveDuration(duration);
    }

    public override void Clear()
    {
        base.Clear();

        ResetCardCostPayload payload = new()
        {
            source = owner,
        };

        foreach (Card card in Battle.Instance.Deck.Hand.CurHand)
        {
            payload.cardInstanceId = card.Id;
            ActionBus.Dispatch(payload);
        }

        owner.EventBus.RemoveEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }
    
    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveDuration();
    }
}