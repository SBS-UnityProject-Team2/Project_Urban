using UnityEngine;

public class GlacialWedge : Attack
{
    [SerializeField] private int remainingTurn = 1; // 빙결 지속 턴 

    public override CardName Name => CardName.GlacialWedge;

    public override int Use(Target target)
    {
        // 1. 데미지 적용 
        target.Damage(damage);

        // 2. 빙결 상태 적용         
        // target.AddConditionStatus(new Stun(remainingTurn));

        return cost;
    }
}