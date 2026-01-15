using System.Threading;
using UnityEngine;

public class Ignition : Attack
{
    [SerializeField] private int burnCount; 

    public override CardName Name => CardName.Ignition;

    public override int Use(Player player, Target target)
    {   
        target.Damage(player, damage, Element.Ruin);
        target.Burn(burnCount);
        
        return curCost;
    }
}