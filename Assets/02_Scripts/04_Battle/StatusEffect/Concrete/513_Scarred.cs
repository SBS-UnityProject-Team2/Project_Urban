public class Scarred : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Scarred;

    public Scarred(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
    }

    private void HandleDamage(EventPayload eventPayload)
    {
        if (!isActive) return;

        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkLossHp,
            source = owner,
        };
        payload.Write(stack);
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);
    }
}