public class Burn : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Burn;

    public Burn(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
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

        RemoveStack();
    }
}