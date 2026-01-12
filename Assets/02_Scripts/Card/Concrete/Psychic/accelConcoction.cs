using UnityEngine;

public class accelConcoction : BuffCard
{
    [SerializeField] private int duration; // 지속 턴 수 << 기존 버프카드들이 상속받는 Turns 랑 겹치는지 확인해봐야함

    public override CardName Name => CardName.accelConcoction;

    public override int Use(Target target)
    {
        // 1. 플레이어 가져오기        
        Player player = target as Player;

        
        // 2. Player에 만들어둔 함수 호출 (3턴 적용)
        player.ActivateAccelConcoction(duration);        

        return cost;
    }
}