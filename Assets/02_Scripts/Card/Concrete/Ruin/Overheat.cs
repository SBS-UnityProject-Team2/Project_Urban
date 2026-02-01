using System.Collections;
using UnityEngine;

public class Overheat : BuffCard
{
    [SerializeField] private int costGain = 2;
    [SerializeField] private int burnCount = 4;
    
    public override CardName Name => CardName.Overheat;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        user.Cost.Increase(costGain);
        user.Burn(burnCount);
    }
}