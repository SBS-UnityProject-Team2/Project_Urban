using UnityEngine;

public class Overheat : BuffCard
{
    [Header("Balance Settings")]
    [SerializeField] private int costGain = 2;   // 회복할 코스트
    [SerializeField] private int burn = 5; // 자신에게 부여할 화상 수치
    
    public override CardName Name => CardName.Overheat;

    public override int Use(Player player, Target target)
    {
        // 1. 플레이어 형변환
        Player user = player as Player;        

        user.Cost.Increase(costGain);

        // 3. 자신에게 화상 디버프 부여
        player.Burn(burn);

        return curCost;
    }
}