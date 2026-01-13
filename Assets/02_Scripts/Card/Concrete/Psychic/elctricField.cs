using UnityEngine;
public class electricField : BuffCard
{   
    [SerializeField] private int damage; 

    public override CardName Name => CardName.electricField;

    public override int Use(Target target)
    {
        // 1. 플레이어 확인        
        Player player = target as Player;

        // 2. Player에 만들어둔 활성화 함수 호출
        player.ElectricVeil(damage);       // 전체 몇데미지 주는지 없음   

        return cost;
    }
}