using UnityEngine;

public class ElasticWall : Defense
{ 
    [SerializeField] private int turn;      // 버프 적용 턴수
    public override CardName Name => CardName.ElasticWall;

    public override int Use(Player player, Target target)
    {
        target.Protect(armor);
        target.BioActiveShell(turn);
        target.ElasticVeil(turn);

        return curCost;
    }
}