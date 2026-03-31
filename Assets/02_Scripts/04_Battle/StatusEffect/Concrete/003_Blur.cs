public class Blur : StackEffect, IDamageNullifier
{
    public override StatusEffectName Name => StatusEffectName.Blur;
    
    public Blur(Actor owner) : base(owner) {}

    public bool TryNullification()
    {
        if (!isActive) return false;
        
        RemoveStack();
        return true;
    }
}