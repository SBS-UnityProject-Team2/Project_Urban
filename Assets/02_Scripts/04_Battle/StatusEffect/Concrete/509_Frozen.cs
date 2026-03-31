public class Frozen : DurationEffect, IActionBlock
{
    public override StatusEffectName Name => StatusEffectName.Frozen;

    public Frozen(Actor owner) : base(owner) {}

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

    public bool IsActionBlocked()
    {
        return isActive;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveStack();
    }
}