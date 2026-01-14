using System.Threading;
using UnityEngine;

public class Ignition : Debuff
{
    [SerializeField] private int burn; 

    public override CardName Name => CardName.Ignition;

    public override int Use(Player player, Target target)
    {
        // 1. 도트 데미지 
        target.Burn(burn);
        
        return curCost;
    }
}