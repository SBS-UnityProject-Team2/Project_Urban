using UnityEngine;

public class cycle : BuffCard
{
    public override CardName Name => CardName.Cycle;

    public override int Use(Player player, Target target)
    {
        return curCost;    // Cycle 은 보류
    }
}