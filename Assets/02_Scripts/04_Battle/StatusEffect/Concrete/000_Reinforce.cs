public class Reinforce : StackEffect, IAttackerDamageFlatChange
{
    public override StatusEffectName Name => StatusEffectName.Reinforce;
    public Reinforce(Actor owner) : base(owner) {}

    public int GetDamageDelta()
    {
        return stack;
    }
}