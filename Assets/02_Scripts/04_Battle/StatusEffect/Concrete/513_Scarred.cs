public class Scarred : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Scarred;

    public Scarred(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)
            owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.DamageTaken, HandleDamage);
    }

    private void HandleDamage(EventPayload eventPayload)
    {
        AtkLossHpPayload payload = new()
        {
            source = owner,
            damage = stack,  
        };
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);
    }
}