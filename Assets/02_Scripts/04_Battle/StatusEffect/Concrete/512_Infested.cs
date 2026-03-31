public class Infested : DurationEffect, IDefenderDamageRateChange
{
    private readonly float damageModifier = 0.5f;
    public override StatusEffectName Name => StatusEffectName.Infested;

    public Infested(Actor owner) : base(owner) {}

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

    public float GetDamageDelta(ElementType elementType)
    {
        if (!isActive) return 0;
        if (elementType != ElementType.Bio) return 0;

        return damageModifier;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveStack();
    }
}