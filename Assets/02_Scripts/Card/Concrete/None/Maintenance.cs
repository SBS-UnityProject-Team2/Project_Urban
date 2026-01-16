using UnityEngine;

public class Maintenance : BuffCard
{
    [SerializeField] private int drawCount = 2; // 드로우

    public override CardName Name => CardName.Maintenance;

    public override int Use(Player player, Target target)
    {
        player.DrawCard(drawCount);

        return curCost;
    }
}