using UnityEngine;

public class Overheat : Buff
{
    [SerializeField] private int costGain = 2; // 회복할 코스트
    [SerializeField] private int burn = 4; // 자신에게 줄 화상
    public override CardName Name => CardName.Overheat;

    public override int Use(Target target)
    {   
        // Player 연결확인
        Player player = target as Player;

        if (player != null)
        {
            player.Cost.IncreaseCost(costGain);

            // 화상 부여
            player.IncreaseBurn(burn);
            
            Debug.Log($"과열 발동: 코스트 +{costGain}, 내 화상 +{burn}");
        }
        
        return cost;
    }
}