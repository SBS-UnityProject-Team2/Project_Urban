public class Broken : TimedStatusEffect
{
    private readonly float damageModifier = 0.3f;

    public Broken() : base(2) {}

    public override void Apply(Target target)
    {
        target.IncreaseDamageTaken(damageModifier);
    }

    public override void Revert(Target target)
    {
        target.DecreaseDamageTaken(damageModifier);
    }
}