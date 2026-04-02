public class Spike : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Spike;

    public Spike(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)
        {
            owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
            owner.EventBus.AddEventListener(ActorEvent.InitDraw, HandleInitDraw);
        }

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.DamageTaken, HandleDamage);
        owner.EventBus.RemoveEventListener(ActorEvent.InitDraw, HandleInitDraw);
    }

    private void HandleDamage(EventPayload eventPayload)
    {
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
        RequestClear();
    }
}