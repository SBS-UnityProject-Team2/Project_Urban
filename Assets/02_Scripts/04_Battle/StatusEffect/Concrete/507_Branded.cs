public class Branded : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Branded;
    
    public Branded(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
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

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RequestClear();
    }
}