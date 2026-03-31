public class KineticVeil : DurationEffect
{
    public override StatusEffectName Name => StatusEffectName.KineticVeil;
    
    public KineticVeil(Actor owner) : base(owner) {}

    public override void GiveDuration(int duration = 1)
    {
        if (!isActive)
        {
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
            
            ChangeElementPayload payload = new()
            {
                source = owner,
                elementType = ElementType.Psychic
            };

            payload.AddTarget(owner);
            ActionBus.Dispatch(payload);
        }

        base.GiveDuration(duration);
    }

    public override void Clear()
    {
        base.Clear();

        owner.EventBus.RemoveEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
        
        ChangeElementPayload payload = new()
        {
            source = owner,
            elementType = ElementType.Reset
        };

        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);
    }
    
    private void HandleTurnEnd(EventPayload eventPayload)
    {
        RemoveDuration();
    }
}