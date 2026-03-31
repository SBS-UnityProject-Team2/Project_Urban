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
        HealHpPayload payload = new()
        {
            source = owner,
            healPoint = stack
        };

        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);

        RemoveStack();
    }
}