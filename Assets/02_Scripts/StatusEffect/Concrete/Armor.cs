public class Armor : InstantStatusEffect
{
    private readonly int count = 2;

    public override void Apply(Target target)
    {
        target.AddProtect(count);
    }
}