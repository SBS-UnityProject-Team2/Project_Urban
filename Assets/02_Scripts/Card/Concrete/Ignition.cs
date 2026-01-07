using UnityEngine;

public class Ignition : Debuff
{   
    [SerializeField] private int damage = 5;
    [SerializeField] private int burnDamage = 1; // 도트 데미지
    [SerializeField] private int remainingTurn = 3;

    public override CardName Name => CardName.Ignition;

    public override int Use(Target target)
    {
        // 1. 즉발 데미지 
        // target.Damage(damage);

        // 2. 도트 데미지 
        // target.AddConditionStatus(new DoTDamage(burnDamage, remainingTurn));
        
        return cost;
    }
}