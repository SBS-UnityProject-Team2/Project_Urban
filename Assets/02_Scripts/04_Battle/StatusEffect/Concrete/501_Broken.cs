public class Broken : DurationEffect, IDefenderDamageRateChange
{
    private readonly float damageModifier = 0.3f;
    public override StatusEffectName Name => StatusEffectName.Weaken;

    public Broken(Actor owner) : base(owner) 
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public float GetDamageDelta(ElementType elementType)
    {
        return damageModifier;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}