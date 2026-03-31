public class ElectricVeil : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.ElectricVeil;

    public ElectricVeil(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        AtkDmgPayload payload = new()
        {
            source = owner,
            damage = stack,
            elementType = ElementType.Psychic  
        };

        if (payload.source == Battle.Instance.Player)
        {
            foreach (Monster monster in Battle.Instance.Monsters)
                payload.AddTarget(monster);
        }
        else
        {
            payload.AddTarget(Battle.Instance.Player);   
        }

        ActionBus.Dispatch(payload);
    }
}