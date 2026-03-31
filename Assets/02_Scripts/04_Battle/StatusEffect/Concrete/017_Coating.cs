public class Coating : StackEffect, IResistEffect
{
    public override StatusEffectName Name => StatusEffectName.Coating;

    public Coating(Actor owner) : base(owner) {}

    public bool Resist(StatusEffectName effectName)
    {
        if (!isActive) return false;

        RemoveStack();
        return true;
    }
}