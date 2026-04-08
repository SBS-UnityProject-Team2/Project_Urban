public class Acceleration : DurationEffect, IDrawCountChange, ICostRegenChange
{
    public override StatusEffectName Name => StatusEffectName.Acceleration;
    
    public Acceleration(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }

    public int GetDrawCountDelta()
    {
        return 1;
    }

    public int GetCostDelta()
    {
        return 1;
    }
}