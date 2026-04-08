public class Searing : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Searing;

    public Searing(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
    }

    private void HandleDamage(EventPayload eventPayload)
    {
        if (!isActive) return;

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