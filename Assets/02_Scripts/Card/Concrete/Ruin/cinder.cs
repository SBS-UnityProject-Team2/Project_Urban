using UnityEngine;

public class Cinder : BuffCard 
{
    [SerializeField] private int drawAmount = 2; 
    public override CardName Name => CardName.Cinder;

    public override int Use(Player player, Target target)
    {        
        // 2. Player에 만들어둔 함수 호출
        player.Searing(drawAmount);      // 근데 잃은체력 얼마당 몇드로우인지가없음 > ???
        return cost;
    }
}