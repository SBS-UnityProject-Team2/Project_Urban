using UnityEngine;
public class Stigma : Debuff
{

    [SerializeField] private int turn;
    public override CardName Name => CardName.Stigma;
    public override int Use(Player player, Target target)
    {
        target.Branded(turn);
        return curCost;
    }
}