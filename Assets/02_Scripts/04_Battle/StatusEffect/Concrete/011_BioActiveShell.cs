public class BioActiveShell : DurationEffect
{
    public override StatusEffectName Name => StatusEffectName.BioActiveShell;

    public BioActiveShell(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public override void GiveDuration(int duration = 1)
    {
        if (!isActive)
        {
            ActionPayload payload = new()
            {
                actionId = ActorAction.ChangeElement,
                source = owner,
            };
            payload.Write(ElementType.Bio);

            payload.AddTarget(owner);
            ActionBus.Dispatch(payload);
        }

        base.GiveDuration(duration);
    }

    public override void Clear()
    {
        base.Clear();

        ActionPayload payload = new()
        {
            actionId = ActorAction.ChangeElement,
            source = owner,
        };
        payload.Write(ElementType.Reset);

        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}