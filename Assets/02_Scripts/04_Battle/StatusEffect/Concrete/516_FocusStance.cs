public class FocusStance : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.FocusStance;

    public FocusStance(Actor owner) : base(owner) {}

    public override void GiveDuration(int duration = 1)
    {
        if (!isActive)
            owner.EventBus.AddEventListener(ActorEvent.Break, HandleBreak);

        base.GiveDuration(duration);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.Break, HandleBreak);
    }

    private void HandleBreak(EventPayload eventPayload)
    {
        ClearBuffsPayload payload = new()
        {
            source = owner,
            effectName = StatusEffectName.Burst
        };
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);

        RequestClear();
    }   
}