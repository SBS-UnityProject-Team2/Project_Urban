using UnityEngine;

public class ElasticWall : Defense
{   
    [SerializeField] private int turn;
    public override CardName Name => CardName.ElasticWall;

    public override int Use(Player player, Target target)
    {
        target.Protect(armor);
        target.BioActiveShell(turn);
        // 탄성막 버프 구현 필요

        return cost;
    }
}