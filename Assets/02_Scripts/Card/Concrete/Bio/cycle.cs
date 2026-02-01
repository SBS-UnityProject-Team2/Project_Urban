using System.Collections;
using UnityEngine;

public class Cycle : BuffCard
{
    public override CardName Name => CardName.Cycle;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        user.ResourceTrade();
    }
}