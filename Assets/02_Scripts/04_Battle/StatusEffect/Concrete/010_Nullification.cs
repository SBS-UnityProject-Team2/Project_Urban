public class Nullification : DurationEffect, IDamageNullifier
{
    public override StatusEffectName Name => StatusEffectName.Nullification;

    public Nullification(Actor owner) : base(owner) {}

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

    public bool TryNullification()
    {
        return isActive;
    }
}