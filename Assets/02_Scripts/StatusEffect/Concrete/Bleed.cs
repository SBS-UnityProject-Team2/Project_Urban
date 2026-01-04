public class Bleed : InstantStatusEffect
{
    private readonly int count;

    public Bleed(int count)
    {
        this.count = count;
    }

    public override void Apply(Target target)
    {
        target.AddBleed(count);
    }
}