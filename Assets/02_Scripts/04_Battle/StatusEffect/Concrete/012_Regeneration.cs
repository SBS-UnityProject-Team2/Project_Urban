public class Regeneration : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Regeneration;

    public Regeneration(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        ActionPayload payload = new()
        {
            actionId = ActorAction.HealHp,
            source = owner,
        };
        payload.Write(stack);

        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);

        RemoveStack();
    }
}