using UnityEngine;

public class ElectricArrow : Attack
{
    [SerializeField] private int drawCount = 1;       // 기본 드로우 수

    public override CardName Name => CardName.ElectricArrow;

    public override int Use(Player player, Target target)
    {
        // 1. 적에게 데미지 주기
        target.Damage(player, damage, Element.None);

        // 2. 드로우 로직 수행
        int finalDrawCount = drawCount;

            // 적이 빙결(Frozen) 상태인지 확인
            if (target.Status.Frozen.IsActive)
            {
                finalDrawCount++; // 1 추가
            }

            // 계산된 수만큼 드로우
            player.DrawCard(finalDrawCount);

        return curCost;
    }
}