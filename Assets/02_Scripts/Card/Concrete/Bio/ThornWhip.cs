using UnityEngine;

public class ThornWhip : Attack
{
    public override CardName Name => CardName.ThornWhip;

    public override int Use(Target target)
    {
        return cost;
    }
}