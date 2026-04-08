public class Poisoned : DurationEffect
{
    private readonly float damageRatio = 0.2f;

    public override StatusEffectName Name => StatusEffectName.Poisoned;

    public Poisoned(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }


    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkLossHp,
            source = owner,
        };
        payload.Write((int)(owner.Status.Health.CurHp * damageRatio));
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);

        RemoveDuration();
    }
}