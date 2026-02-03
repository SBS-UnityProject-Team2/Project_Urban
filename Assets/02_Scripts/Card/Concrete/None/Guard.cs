using System.Collections;

public class Guard : Defense
{
    public override CardName Name => CardName.Guard;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Protect(protect);
    }
}