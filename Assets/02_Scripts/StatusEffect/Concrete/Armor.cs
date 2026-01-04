public class Armor : InstantStatusEffect
{
    private readonly int count;

    public Armor(int count)
    {
        this.count = count;
    }

    public override void Apply(Target target)
    {
        target.AddProtect(count);
    }
}