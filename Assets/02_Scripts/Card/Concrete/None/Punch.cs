using System.Collections;
using UnityEngine;

public class Punch : Attack
{
    public override CardName Name => CardName.Punch;
    
    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Damage(user, damage);
    }
}