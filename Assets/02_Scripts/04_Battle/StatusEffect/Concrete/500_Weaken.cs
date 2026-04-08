public class Weaken : DurationEffect, IAttackerDamageRateChange
{
    private readonly float damageModifier = 0.3f;
    public override StatusEffectName Name => StatusEffectName.Weaken;

    public Weaken(Actor owner) : base(owner) 
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public float GetDamageDelta()
    {
        return -damageModifier;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}