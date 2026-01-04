public class Reinforce : InstantStatusEffect
{
    private readonly int count;

    public Reinforce(int count)
    {
        this.count = count;
    }

    public override void Apply(Target target)
    {
        target.IncreaseAttack(count);
    }
}