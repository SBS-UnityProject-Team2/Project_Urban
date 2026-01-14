using UnityEngine;

public class DistortedSlay : Attack
{
    public override CardName Name => CardName.DistortedSlay;

    public override int Use(Player player, Target target)
    {   
        // 버려진카드더미? << 버리는 기믹으로 버린 카드 수를 정확하게 알아야하는지?
        return curCost;
    }
}