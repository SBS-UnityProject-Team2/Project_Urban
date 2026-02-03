using UnityEngine;
using System.Collections;

public class Dummy : BuffCard
{
    [SerializeField] private int blurPoint;
    public override CardName Name => CardName.Dummy;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Blur(blurPoint);
    }
}