using UnityEngine;

public class CellChange : Attack
{
    public override CardName Name => CardName.CellChange;

    public override int Use(Target target)
    {
        return cost;
    }
}