public class Exhaust : DurationEffect, ICostRegenChange
{
    private readonly int decreaseCost = 1;
    public override StatusEffectName Name => StatusEffectName.Exhaust;

    public Exhaust(Actor owner) : base(owner) {}

    public override void GiveDuration(int duration = 1)
    {
        if (!isActive)
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);

        base.GiveDuration(duration);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public int GetCostDelta()
    {
        return decreaseCost;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveStack();
    }
}