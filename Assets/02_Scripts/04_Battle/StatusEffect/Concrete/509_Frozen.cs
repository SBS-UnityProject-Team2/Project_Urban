public class Frozen : DurationEffect, IActionBlock
{
    public override StatusEffectName Name => StatusEffectName.Frozen;

    public Frozen(Actor owner) : base(owner) 
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);

    }

    public bool IsActionBlocked()
    {
        return isActive;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}