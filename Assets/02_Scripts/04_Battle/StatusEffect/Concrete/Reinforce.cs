public class Reinforce : StackStatusEffect
{
    public Reinforce(Actor owner) : base(owner)
    {
    }

    public override int StatusNumber => Stack;
    public override StatusEffectName Name => StatusEffectName.Reinforce;

    public int ApplyDamage(int originDamage)
    {
        return originDamage + stack;
    }
}