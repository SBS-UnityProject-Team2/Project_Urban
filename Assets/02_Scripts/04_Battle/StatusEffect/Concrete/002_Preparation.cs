public class Preparation : StackEffect, IDrawCountChange
{
    public override StatusEffectName Name => StatusEffectName.Preparation;
    
    public Preparation(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.InitDraw, HandleInitDraw);
    }

    public int GetDrawCountDelta()
    {
        return stack;
    }

    private void HandleInitDraw(EventPayload payload)
    {
        if (!isActive) return;

        RequestClear();
    }   
}