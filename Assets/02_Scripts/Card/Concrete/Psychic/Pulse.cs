using UnityEngine;

public class Pulse : Attack
{
    [SerializeField] private int bonusDamage = 6;    // 동결 시 추가 데미지

    public override CardName Name => CardName.Pulse;

    public override int Use(Target target)
    {
        int finalDamage = damage;       // 최종 데미지 계산

        if (target.Status.IsFrozen)     // 적이 동결상태인지 확인
        {
            finalDamage += bonusDamage;
        }

        Player player = BattleManager.Instance.Player;

        target.Damage(player, finalDamage);

        return cost;
    }
}