using UnityEngine;

public class Cycle : BuffCard
{
    public override CardName Name => CardName.Cycle;

    public override int Use(Player player, Target target)
    {
        player.ResourceTrade();
        
        return curCost;    // Cycle 은 보류
    }
}