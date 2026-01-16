using UnityEngine;

public class Overheat : BuffCard
{
    [SerializeField] private int costGain = 2;   // 회복할 코스트
    [SerializeField] private int burnCount = 5; // 자신에게 부여할 화상 수치
    
    public override CardName Name => CardName.Overheat;

    public override int Use(Player player, Target target)
    {
        player.Cost.Increase(costGain);
        player.Burn(burnCount);

        return curCost;
    }
}