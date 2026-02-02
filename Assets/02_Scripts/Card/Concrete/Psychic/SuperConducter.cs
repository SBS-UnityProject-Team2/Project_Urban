using System.Collections;
using UnityEngine;

public class SuperConducter : BuffCard
{     
    public override CardName Name => CardName.SuperConducter;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Nullification(turns);
        user.Frozen(turns);
        user.KineticVeil(turns);
    }
}