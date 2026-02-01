using System.Collections;

public class Guard : Defense
{
    private readonly int value = 5;
    public override CardName Name => CardName.Guard;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Protect(value);
    }
}