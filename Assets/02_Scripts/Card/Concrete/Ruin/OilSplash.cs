using System.Collections;
using UnityEngine;

public class OilSplash : Debuff
{
    public override CardName Name => CardName.OilSplash;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        
        Burn burn = target.Status.Burn;
        if (burn.IsActive)
            target.Burn(burn.Count * 2);
    }
}