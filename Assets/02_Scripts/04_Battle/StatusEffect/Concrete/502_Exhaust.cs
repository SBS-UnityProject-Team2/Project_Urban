public class Exhaust : DurationEffect, ICostRegenChange
{
    private readonly int decreaseCost = 1;
    public override StatusEffectName Name => StatusEffectName.Exhaust;

    public Exhaust(Actor owner) : base(owner) 
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public int GetCostDelta()
    {
        return decreaseCost;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;
        
        RemoveDuration();
    }
}