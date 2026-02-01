using System.Collections;
using System.Threading;
using UnityEngine;

public class Ignition : Attack
{
    [SerializeField] private int burnCount; 

    public override CardName Name => CardName.Ignition;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {   
        yield return PlayEffect(target);
        target.Damage(user, damage, Element.Ruin);
        target.Burn(burnCount);
    }
}