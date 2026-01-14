using UnityEngine;

public class Punch : Attack
{
    public override CardName Name => CardName.Punch;
    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage);

        return curCost;
    }
}