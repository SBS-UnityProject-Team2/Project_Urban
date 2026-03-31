public class Weaken : DurationEffect, IAttackerDamageRateChange
{
    private readonly float damageModifier = 0.3f;
    public override StatusEffectName Name => StatusEffectName.Weaken;

    public Weaken(Actor owner) : base(owner) {}

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

    public float GetDamageDelta()
    {
        return -damageModifier;
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveStack();
    }
}