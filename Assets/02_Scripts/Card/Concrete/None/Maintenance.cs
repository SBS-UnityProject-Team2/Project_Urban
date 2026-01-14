using UnityEngine;

public class Maintenance : BuffCard
{
    [SerializeField] private int drawCount = 2; // 드로우

    public override CardName Name => CardName.Maintenance;
    public override int Use(Player player, Target target)
    {
        // 1. Player로 형변환
        Player user = player as Player;

        // 2. 드로우 실행
        user.DrawCard(drawCount);


        return curCost;
    }
}