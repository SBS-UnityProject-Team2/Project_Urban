public class Delirium : DurationEffect, IDefenderDamageRateChange
{
    private readonly float damageModifier = 0.5f;
    public override StatusEffectName Name => StatusEffectName.Delirium;

    public Delirium(Actor owner) : base(owner) 
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public float GetDamageDelta(ElementType elementType)
    {
        if (elementType != ElementType.Psychic) return 0;

        return damageModifier;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}