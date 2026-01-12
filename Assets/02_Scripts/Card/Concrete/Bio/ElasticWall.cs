using UnityEngine;

public class ElasticWall : Defense
{   
    [SerializeField] private int turn;
    public override CardName Name => CardName.ElasticWall;

    public override int Use(Target target)
    {
        target.Protect(armor);
        target.BioActiveShell(turn);
        // 탄성막이 어떤건지모르겠음;;;

        return cost;
    }
}