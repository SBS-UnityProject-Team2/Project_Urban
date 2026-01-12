using UnityEngine;

public class Blooming : Attack
{
    public override CardName Name => CardName.Blooming;

    public override int Use(Target target)
    {
        return cost;
    }
}