public class Branded : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.Branded;
    
    public Branded(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)
        {   
            owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
        }

        base.GiveStack(stack); 
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.DamageTaken, HandleDamage);
        owner.EventBus.RemoveEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleDamage(EventPayload eventPayload)
    {
        AtkLossHpPayload payload = new()
        {
            source = owner,
            damage = stack
        };
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RequestClear();
    }
}