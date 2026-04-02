public class LoadedIncendiary : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.LoadedIncendiary;

    public LoadedIncendiary(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)
        {
            owner.EventBus.AddEventListener(ActorEvent.Attack, HandleAttack);
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
        }

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.Attack, HandleAttack);
        owner.EventBus.RemoveEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleAttack(EventPayload eventPayload)
    {
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
        RequestClear();
    }
}