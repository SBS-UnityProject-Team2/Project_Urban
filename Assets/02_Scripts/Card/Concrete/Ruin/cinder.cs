using UnityEngine;


public class Cinder : BuffCard 
{   
    [SerializeField] private int count;     
    public override CardName Name => CardName.Cinder;
    public override int Use(Player player, Target target)
    {
        target.Searing(count);      // 잃은체력 몇당 몇드로우인지가 없음

        return curCost;
    }
}
