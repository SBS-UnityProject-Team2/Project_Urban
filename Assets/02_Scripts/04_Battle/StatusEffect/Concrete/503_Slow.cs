public class Slow : DurationEffect, IDrawCountChange
{
    private readonly int decreaseDrawCount = 1;
    public override StatusEffectName Name => StatusEffectName.Slow;
    
    public Slow(Actor owner) : base(owner) 
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public int GetDrawCountDelta()
    {
        return decreaseDrawCount;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}