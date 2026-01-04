public class Dummy : InstantStatusEffect
{
    private readonly int count;

    public Dummy(int count)
    {
        this.count = count;
    }

    public override void Apply(Target target)
    {
        target.AddBlock(count);
    }
}