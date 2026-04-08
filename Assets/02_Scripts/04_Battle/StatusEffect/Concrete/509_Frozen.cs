public class Frozen : DurationEffect, IActionBlock
{
    public override StatusEffectName Name => StatusEffectName.Frozen;

    public Frozen(Actor owner) : base(owner) 
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    public bool IsActionBlocked()
    {
        return isActive;
    }

    public override void Clear()
    {
        base.Clear();

        ActionPayload payload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = owner,
        };
        payload.AddTarget(owner);
        payload.Write(StatusEffectName.FrozenResistance);
        payload.Write(3);

        ActionBus.Dispatch(payload);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RemoveDuration();
    }
}