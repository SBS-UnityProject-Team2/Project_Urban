public class Armor : StackEffect, IBlockChange
{
    public override StatusEffectName Name => StatusEffectName.Armor;
    public Armor(Actor owner) : base(owner) {}

    public int GetBlockDelta()
    {
        return stack;
    }
}