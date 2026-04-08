public class ElasticVeil : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.ElasticVeil;

    public ElasticVeil(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
        owner.EventBus.AddEventListener(ActorEvent.InitDraw, HandleInitDraw);
    }

    private void HandleDamage(EventPayload eventPayload)
    {
        if (!isActive) return;

        GiveBuff(eventPayload, StatusEffectName.Weaken);
        GiveBuff(eventPayload, StatusEffectName.Broken);
    }

    private void HandleInitDraw(EventPayload eventPayload)
    {
        if (!isActive) return;

        RequestClear();
    }

    private void GiveBuff(EventPayload eventPayload, StatusEffectName effectName)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = eventPayload.target,
        };
        payload.Write(effectName);
        payload.Write(stack);
        
        payload.AddTarget(eventPayload.source);
        ActionBus.Dispatch(payload);
    }
}