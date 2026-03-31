public class Preparation : StackEffect, IDrawCountChange
{
    public override StatusEffectName Name => StatusEffectName.Preparation;
    
    public Preparation(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)
            owner.EventBus.AddEventListener(ActorEvent.InitDraw, HandleInitDraw);

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.InitDraw, HandleInitDraw);
    }

    public int GetDrawCountDelta()
    {
        return stack;
    }

    private void HandleInitDraw(EventPayload payload)
    {
        RequestClear();
    }   
}