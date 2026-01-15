using UnityEngine;

public class ThornWhip : Attack 
{   
    [SerializeField] private int turn;
    public override CardName Name => CardName.ThornWhip;

    public override int Use(Player player, Target target)
    { 
        // 적에게 데미지 입히기
        target.Damage(player, damage, Element.Bio);
        target.Broken(turn);
        return curCost;
    }
}