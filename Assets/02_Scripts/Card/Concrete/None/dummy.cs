using System.Collections;

public class Dummy : BuffCard
{
    public override CardName Name => CardName.Dummy;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Blur(turns);
    }
}