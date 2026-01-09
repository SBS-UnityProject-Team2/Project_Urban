using UnityEngine;

public class Ignition : Debuff
{   
    [SerializeField] private int damage = 6;
    [SerializeField] private int burn = 2; 


    public override CardName Name => CardName.Ignition;

    public override int Use(Target target)
    {
        // 1. 즉발 데미지 
        // target.Damage(6);

        // 2. 도트 데미지 
        target.IncreaseBurn(burn);
        
        return cost;
    }
}