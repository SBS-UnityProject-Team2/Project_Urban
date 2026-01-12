using UnityEngine;

public class DoubleEdge : Attack
{
    [SerializeField] private int selfDamage = 3; // 나에게 입힐 데미지

    public override CardName Name => CardName.DoubleEdge;

    public override int Use(Target target)
    { 
        // // 2. 반동 데미지        
        // BattleManager.Instance.Player.Damage(selfDamage);

        // 3. 코스트 반환
        return cost;
    }
}