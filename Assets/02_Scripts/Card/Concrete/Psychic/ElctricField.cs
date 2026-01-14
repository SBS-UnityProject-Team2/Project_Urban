using UnityEngine;

public class ElectricField : BuffCard
{   
    [SerializeField] private int damage; 

    public override CardName Name => CardName.ElectricField;

    public override int Use(Player player, Target target)
    {
        // 1. Player에 만들어둔 활성화 함수 호출
        player.ElectricVeil(damage);       // 전체 몇데미지 주는지 없음   

        return curCost;
    }
}