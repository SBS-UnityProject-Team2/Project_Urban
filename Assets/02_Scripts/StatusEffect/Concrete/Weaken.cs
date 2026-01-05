public class Weaken : TimedStatusEffect
{
    private readonly float weakenRatio = 0.3f;

    public Weaken() : base(2) {}

    public override void Apply(Target target)
    {
        target.IncreaseAttack(weakenRatio);
    }

    public override void Revert(Target target)
    {
        target.DecreaseAttack(weakenRatio);
    }
}