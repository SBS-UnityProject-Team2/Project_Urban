public class FrozenResistance : DurationEffect, IResistEffect
{
    public override StatusEffectName Name => StatusEffectName.FrozenResistance;
    public FrozenResistance(Actor owner) : base(owner) {}

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

    public bool Resist(StatusEffectName effectName)
    {
        if (!isActive) return false;
        if (effectName != StatusEffectName.Frozen) return false;

        return true;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveStack();
    }
}