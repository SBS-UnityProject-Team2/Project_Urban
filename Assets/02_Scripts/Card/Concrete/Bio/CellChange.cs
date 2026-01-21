using UnityEngine;
using System.Collections.Generic;
public class CellChange : BuffCard
{
    [SerializeField] private int maxDiscard = 2; 
    [SerializeField] private int drawCount = 4;

    public override CardName Name => CardName.CellChange;

    public override int Use(Player player, Target target)
    {
        player.DiscardCard(maxDiscard, maxDiscard, _ => player.DrawCard(drawCount));

        return curCost;
    }
}