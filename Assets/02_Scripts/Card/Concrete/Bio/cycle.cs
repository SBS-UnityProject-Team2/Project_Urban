using UnityEngine;

public class cycle : BuffCard
{
    public override CardName Name => CardName.cycle;

    public override int Use(Target target)
    {
        // 카드 1장 선택해서 usedCardList 로 보내고, 체력, 코스트 회복
        return cost;
    }
}