using UnityEngine;

public class SpikyBush : Attack
{
    public override CardName Name => CardName.SpikyBush;

    public override int Use(Target target)
    {
        return cost;
    }
}