public class LoadedIncendiary : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.LoadedIncendiary;

    public LoadedIncendiary(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.Attack, HandleAttack);
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleAttack(EventPayload eventPayload)
    {
        if (!isActive) return;

        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkDmg,
            source = owner,
        };
        payload.Write(ElementType.Ruin);
        payload.Write(stack);

        payload.AddTarget(eventPayload.target);
        ActionBus.Dispatch(payload);
    }
    
    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        RequestClear();
    }
}