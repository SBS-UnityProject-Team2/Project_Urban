public class FocusStance : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.FocusStance;

    public FocusStance(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.Break, HandleBreak);
    }

    private void HandleBreak(EventPayload eventPayload)
    {
        if (!isActive) return;

        ActionPayload payload = new()
        {
            actionId = ActorAction.ClearBuffs,
            source = owner,
        };
        payload.Write(StatusEffectName.Burst);
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);

        RequestClear();
    }   
}