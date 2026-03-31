public class ElasticVeil : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.ElasticVeil;

    public ElasticVeil(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
    }

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)
        {
            owner.EventBus.AddEventListener(ActorEvent.DamageTaken, HandleDamage);
            owner.EventBus.AddEventListener(ActorEvent.InitDraw, HandleInitDraw);
        }

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.DamageTaken, HandleDamage);
        owner.EventBus.RemoveEventListener(ActorEvent.InitDraw, HandleInitDraw);
    }

    private void HandleDamage(EventPayload eventPayload)
    {
        GiveBuff(eventPayload, StatusEffectName.Weaken);
        GiveBuff(eventPayload, StatusEffectName.Broken);
    }

    private void HandleInitDraw(EventPayload eventPayload)
    {
        RequestClear();
    }

    private void GiveBuff(EventPayload eventPayload, StatusEffectName effectName)
    {
        GiveBuffDurPayload payload = new()
        {
            effectName = effectName,
            duration = stack,
            source = eventPayload.target,  
        };
        
        payload.AddTarget(eventPayload.source);
        ActionBus.Dispatch(payload);
    }
}