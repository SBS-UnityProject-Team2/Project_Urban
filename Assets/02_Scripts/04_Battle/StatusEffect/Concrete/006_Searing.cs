public class Searing : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Searing;

    public Searing(Actor owner) : base(owner) {}

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
        ActionPayload payload = new()
        {
            actionId = ActorAction.MoveCardFromDeck,
            source = owner,
        };
        payload.Write(Location.Hand);
        payload.Write(stack);

        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);
    }
}