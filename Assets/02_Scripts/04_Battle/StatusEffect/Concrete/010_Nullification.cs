public class Nullification : DurationEffect, IDamageNullifier
{
    public override StatusEffectName Name => StatusEffectName.Nullification;

    public Nullification(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }

    public bool TryNullification()
    {
        return isActive;
    }
}