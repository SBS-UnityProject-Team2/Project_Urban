using System.Collections;
using UnityEngine;

public class HeatUp : BuffCard
{
    [SerializeField] private int reinforcePoint = 2;
    public override CardName Name => CardName.HeatUp;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Reinforce(reinforcePoint);
    }
}