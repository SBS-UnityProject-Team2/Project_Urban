public class FrozenResistance : DurationEffect, IResistEffect
{
    public override StatusEffectName Name => StatusEffectName.FrozenResistance;
    public FrozenResistance(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public bool Resist(StatusEffectName effectName)
    {
        if (!isActive) return false;
        if (effectName != StatusEffectName.Frozen) return false;

        return true;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}