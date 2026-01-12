using UnityEngine;

public class cycle : Attack
{
    public override CardName Name => CardName.cycle;

    public override int Use(Target target)
    {
        return cost;
    }
}