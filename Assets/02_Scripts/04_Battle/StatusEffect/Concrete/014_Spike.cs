public class Spike : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Spike;

    public Spike(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
        owner.EventBus.AddEventListener(ActorEvent.InitDraw, HandleInitDraw);
    }

    private void HandleDamage(EventPayload eventPayload)
    {
        if (!isActive) return;

        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkDmg,
            source = owner,
        };
        payload.Write(ElementType.None);
        payload.Write(stack);
        payload.AddTarget(eventPayload.source);
        ActionBus.Dispatch(payload);
    }

    private void HandleInitDraw(EventPayload eventPayload)
    {
        if (!isActive) return;

        RequestClear();
    }
}