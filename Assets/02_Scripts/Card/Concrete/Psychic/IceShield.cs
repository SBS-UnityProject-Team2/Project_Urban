using UnityEngine;

public class IceShield : Defense
{   
    [SerializeField] private int turn;
    public override CardName Name => CardName.IceShield;

    public override int Use(Player player, Target target)
    {
        // 1. 방어도 추가
        target.Protect(armor);
        target.KineticVeil(turn);
        
        return cost;
    }
}