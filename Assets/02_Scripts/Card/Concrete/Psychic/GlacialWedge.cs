using UnityEngine;

public class GlacialWedge : Attack
{
    [SerializeField] private int turn; // 빙결 지속 턴 

    public override CardName Name => CardName.GlacialWedge;

    public override int Use(Player player, Target target)
    {   
        target.Frozen(turn);        

        return cost;
    }
}