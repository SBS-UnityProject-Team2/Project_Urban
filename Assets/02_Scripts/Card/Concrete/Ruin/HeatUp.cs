using System.Collections;
using UnityEngine;

public class HeatUp : BuffCard
{
    [SerializeField] private int count;
    public override CardName Name => CardName.HeatUp;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Reinforce(count);
    }
}