using UnityEngine;

public class SuperConducter : BuffCard
{     
    public override CardName Name => CardName.SuperConducter;

    public override int Use(Player player, Target target)
    {

        target.Nullification(turns);
        player.Frozen(turns);
        player.KineticVeil(turns);
        
        return curCost;
    }
}