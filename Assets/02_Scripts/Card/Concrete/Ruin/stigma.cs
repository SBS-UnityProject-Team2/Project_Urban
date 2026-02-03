using System.Collections;
using UnityEngine;

public class Stigma : Debuff
{
    [SerializeField] private int brandedPoint = 1;

    public override CardName Name => CardName.Stigma;
    
    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Branded(brandedPoint);
    }
}