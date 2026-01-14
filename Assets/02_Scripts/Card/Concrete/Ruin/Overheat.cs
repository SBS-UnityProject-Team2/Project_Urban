    using UnityEngine;

    public class Overheat : BuffCard
    {
    [SerializeField] private int costGain = 2;   // 회복할 코스트 (value1)
    [SerializeField] private int burnAmount = 5; // 부여할 화상 수치 (value2)
    
    public override CardName Name => CardName.Overheat;

        public override int Use(Player player, Target target)
        {
            // 1. 코스트 회복      
            Player player = target as Player;       
            
            player.Cost.Increase(costGain);       

            // 2. 화상 상태이상 부여        
            target.Burn(burnAmount);       

            return cost;
        }
    }