public class Burn : InstantStatusEffect
{
    private readonly int count;

    public Burn(int count)
    {
        this.count = count;
    }

    public override void Apply(Target target)
    {
        target.IncreaseBurn(count);
    }
}