using UnityEngine;
using System.Collections.Generic;

public class Blooming : BuffCard
{
    [SerializeField] private int costGain = 2; // 회복할 코스트 양
    [SerializeField] private int maxDiscard = 1;

    public override CardName Name => CardName.Blooming;

    public override int Use(Player player, Target target)
    {
        player.DiscardCard(maxDiscard, maxDiscard, _ => player.Cost.Increase(costGain));
        return curCost;
    }
}