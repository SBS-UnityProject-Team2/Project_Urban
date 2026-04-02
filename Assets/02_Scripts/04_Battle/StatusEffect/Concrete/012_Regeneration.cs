public class Regeneration : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Regeneration;

    public Regeneration(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)  
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
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