using UnityEngine;

public class DistortedSlay : Attack
{
    public override CardName Name => CardName.DistortedSlay;

    public override int Use(Target target)
    {
        return cost;
    }
}