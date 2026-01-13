using UnityEngine;

public class DoubleEdge : Attack
{
    [SerializeField] private int selfDamage;   // 반동데미지

    public override CardName Name => CardName.DoubleEdge;

    public override int Use(Target target)
    {
        // 1. 플레이어 가져오기
        Player player = BattleManager.Instance.Player;

        // 2. 적에게 데미지 입히기
        target.Damage(player, damage);

        // 3. 반동 데미지 입히기
        player.Damage(player, selfDamage);

        return cost;
    }
}