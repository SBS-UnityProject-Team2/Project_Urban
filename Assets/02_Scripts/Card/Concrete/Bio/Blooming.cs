using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Blooming : BuffCard
{
    [SerializeField] private int costGain = 2;
    [SerializeField] private int maxDiscard = 1;

    public override CardName Name => CardName.Blooming;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        user.DiscardCard(maxDiscard, maxDiscard, _ => user.Cost.Increase(costGain));
    }
}