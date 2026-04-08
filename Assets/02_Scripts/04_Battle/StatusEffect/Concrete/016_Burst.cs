public class Burst : StackEffect, IAttackerDamageFlatChange
{
    public override StatusEffectName Name => StatusEffectName.Burst;

    public Burst(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.Attack, HandleAttack);
    }

    private void HandleAttack(EventPayload payload)
    {
        if (!isActive) return;

        RequestClear();
    }

    public int GetDamageDelta()
    {
        return stack;
    }
}