using System.Collections;
using UnityEngine;

public class SuperConducter : BuffCard
{     
    [SerializeField] private int nullificationPoint;
    [SerializeField] private int frozenPoint;
    [SerializeField] private int kineticVeilPoint;
    public override CardName Name => CardName.SuperConducter;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Nullification(nullificationPoint);
        user.Frozen(frozenPoint);
        user.KineticVeil(kineticVeilPoint);
    }
}