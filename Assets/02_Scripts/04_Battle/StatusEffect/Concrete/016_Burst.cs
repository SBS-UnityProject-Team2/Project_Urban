public class Burst : StackEffect, IAttackerDamageFlatChange
{
    public override StatusEffectName Name => StatusEffectName.Burst;

    public Burst(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive) 
            owner.EventBus.AddEventListener(ActorEvent.Attack, HandleAttack);

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.Attack, HandleAttack);

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