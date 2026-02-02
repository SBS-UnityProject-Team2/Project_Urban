using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class CellChange : BuffCard
{
    [SerializeField] private int maxDiscard = 2; 
    [SerializeField] private int drawCount = 4;

    public override CardName Name => CardName.CellChange;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        user.DiscardCard(maxDiscard, maxDiscard, _ => user.DrawCard(drawCount));
    }
}