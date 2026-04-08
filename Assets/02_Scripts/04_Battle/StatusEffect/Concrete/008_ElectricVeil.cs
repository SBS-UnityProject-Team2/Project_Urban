public class ElectricVeil : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.ElectricVeil;

    public ElectricVeil(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkDmg,
            source = owner,
        };
        payload.Write(ElementType.Psychic);
        payload.Write(stack);

        if (payload.source == Battle.Instance.Player)
        {
            foreach (Monster monster in Battle.Instance.Monsters.List)
                payload.AddTarget(monster);
        }
        else
        {
            payload.AddTarget(Battle.Instance.Player);   
        }

        ActionBus.Dispatch(payload);
    }
}