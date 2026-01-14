using UnityEngine;

public class EnergyNeedle : Attack
{
    [SerializeField] private int costGain = 1;   // 기본 코스트회복량

    public override CardName Name => CardName.EnergyNeedle;

    public override int Use(Player player, Target target)
    {
        Player user = player as Player;

        // 1. 적에게 데미지 입히기
        target.Damage(player, damage, Element.Psychic);

        // 2. 코스트 회복량 계산
        int finalGain = costGain;

        // 현재코스트 0인지 확인
        if (user.Cost.CurrentCost - curCost == 0)
        {
            finalGain += 1; // 조건 만족 시 1 추가
        }

        // 3. 코스트 회복 적용
        user.Cost.Increase(finalGain);

        return curCost;
    }
}