using UnityEngine;

public class Punch : Attack
{
    public override CardName Name => CardName.Punch;
    public override int Use(Player player, Target target)
    {
        // 이펙트 재생
        //PlayEffect();
        
        // 데미지 적용
        target.Damage(player, damage);

        return curCost;
    }
}