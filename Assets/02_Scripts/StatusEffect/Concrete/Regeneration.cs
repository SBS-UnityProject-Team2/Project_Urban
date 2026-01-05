public class Regeneration : InstantStatusEffect
{
    private readonly int count = 4;

    public override void Apply(Target target)
    {
        target.IncreaseRegeneration(count);
    }
}