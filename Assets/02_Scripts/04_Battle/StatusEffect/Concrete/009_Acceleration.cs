public class Acceleration : DurationEffect, IDrawCountChange, ICostChange
{
    public override StatusEffectName Name => StatusEffectName.Acceleration;
    
    public Acceleration(Actor owner) : base(owner) {}

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

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveStack();
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