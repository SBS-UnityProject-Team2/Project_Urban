using UnityEngine;

public class MoltenArms : Attack 
{
    [SerializeField] private int burnCount;

    public override CardName Name => CardName.MoltenArms;

    public override int Use(Player player, Target target)
    {   
        target.Damage(player, damage, Element.Ruin);
        player.Burn(burnCount);

        return curCost;
    }
}