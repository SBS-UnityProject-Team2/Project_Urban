using System.Collections;

public class Anxiolytic : BuffCard
{
    public override CardName Name => CardName.Anxiolytic;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
    }
}