using UnityEngine;

public class ElectricArrow : Attack
{
    private int amount = 1;
    public override CardName Name => CardName.ElectricArrow;

    public override int Use(Target target)
    {
        // 플레이어 받아옴
        Player player = BattleManager.Instance.Player;



        // 2. 적에게 데미지 주기
        target.Damage(player, damage);

        // 3. 적이 동결 상태인지 확인 후 드로우
        if (target.Status.IsFrozen)
        {
            player.DrawCard(amount);
        }


        return cost;
    }
}