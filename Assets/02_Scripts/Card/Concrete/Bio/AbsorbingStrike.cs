using UnityEngine;

public class AbsorbingStrike : Attack
{
    public override CardName Name => CardName.AbsorbingStrike;

    public override int Use(Target target)
    {
        Player player = BattleManager.Instance.Player;

        // 1. 공격 전, 적몬스터 체력 확인
        int hpBefore = target.Health.CurrentHp;

        // 2. 공격 실행
        target.Damage(player, damage);

        // 3. 공격 후, 적몬스터 현재 체력 확인
        int hpAfter = target.Health.CurrentHp;

        // 4. 실제로 깎인 체력 계산 
        int actualDamageDealt = hpBefore - hpAfter;

        // 5. 깎은 체력만큼 플레이어 회복
        if (actualDamageDealt > 0)
        {
            player.Heal(actualDamageDealt);
        }
        return cost;
    }
}