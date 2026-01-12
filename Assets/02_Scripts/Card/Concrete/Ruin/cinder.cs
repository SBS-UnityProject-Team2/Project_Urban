using UnityEngine;

public class cinder : BuffCard 
{
    [SerializeField] private int drawAmount = 2; 
    public override CardName Name => CardName.cinder;

    public override int Use(Target target)
    {
        // 1. 플레이어 확인
        Player player = target as Player;
        
        // 2. Player에 만들어둔 함수 호출
        player.ActivateCinder(drawAmount);      // 근데 잃은체력 얼마당 몇드로우인지가없음 > ???
        return cost;
    }
}