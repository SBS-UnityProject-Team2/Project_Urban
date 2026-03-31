public class Slow : DurationEffect, IDrawCountChange
{
    private readonly int decreaseDrawCount = 1;
    public override StatusEffectName Name => StatusEffectName.Slow;
    
    public Slow(Actor owner) : base(owner) {}

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

    public int GetDrawCountDelta()
    {
        return decreaseDrawCount;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveStack();
    }
}