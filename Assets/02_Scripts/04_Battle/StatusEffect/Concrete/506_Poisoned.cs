public class Poisoned : DurationEffect
{
    private readonly float damageRatio = 0.2f;

    public override StatusEffectName Name => StatusEffectName.Poisoned;

    public Poisoned(Actor owner) : base(owner) {}

    public override void GiveDuration(int duration = 1)
    {   
        if (!isActive)
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);

        base.GiveDuration(duration);
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
            actionId = ActorAction.AtkLossHp,
            source = owner,
        };
        payload.Write((int)(owner.Status.Health.CurHp * damageRatio));
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);

        RemoveStack();
    }
}