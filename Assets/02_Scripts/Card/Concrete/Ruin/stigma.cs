using UnityEngine;
public class stigma : Debuff
{

    [SerializeField] private int turn;
    public override CardName Name => CardName.Stigma;
    public override int Use(Target target)
    {
        target.Stigma(turn);
        return cost;
    }
}