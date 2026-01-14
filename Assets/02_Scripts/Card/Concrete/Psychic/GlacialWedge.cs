using UnityEngine;

public class GlacialWedge : Attack
{
    [SerializeField] private int turn; // 빙결 지속 턴 

    public override CardName Name => CardName.GlacialWedge;

    public override int Use(Player player, Target target)
    {
        // 1. 적에게 데미지 입히기
        target.Damage(player, damage, Element.Psychic);
        target.Frozen(turn);        

        return curCost;
    }
}