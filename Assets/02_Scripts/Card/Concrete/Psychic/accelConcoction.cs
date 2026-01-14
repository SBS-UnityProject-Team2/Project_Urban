using UnityEngine;

public class accelConcoction : BuffCard
{
    [SerializeField] private int duration;

    public override CardName Name => CardName.AccelConcoction;

    public override int Use(Player player, Target target)
    {        
        // Player에 만들어둔 함수 호출 (3턴 적용)
        player.Acceleration(duration);        

        return curCost;
    }
}