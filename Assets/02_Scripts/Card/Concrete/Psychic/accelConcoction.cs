using UnityEngine;

public class AccelConcoction : BuffCard
{
    [SerializeField] private int duration; // 지속 턴 수 << 기존 버프카드들이 상속받는 Turns 랑 겹치는지 확인해봐야함

    public override CardName Name => CardName.accelConcoction;

    public override int Use(Player player, Target _)
    {
        // Player에 만들어둔 함수 호출 (3턴 적용)
        player.Acceleration(duration);        

        return cost;
    }
}