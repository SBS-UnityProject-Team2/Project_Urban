using UnityEngine;

public class EnergyNeedle : Attack
{
    [SerializeField] private int costGain;   // 회복할 코스트

    public override CardName Name => CardName.EnergyNeedle;

    public override int Use(Target target)
    {
        Player player = BattleManager.Instance.Player;      // 배틀매니저 통해 플레이어 연결

        // 추가 회복 조건 확인 
        int finalGain = costGain;

        if (player.Cost.CurrentCost == 0)
        {
            finalGain += 1; // 조건 만족 시 1 추가            
        }

        // 3. 코스트 회복 적용
        player.Cost.Increase(finalGain);       
        
        target.Damage(player, damage);

        return cost;
    }
}