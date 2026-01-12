using UnityEngine;

public class AbsorbingStrike : Attack
{
    public override CardName Name => CardName.AbsorbingStrike;

    public override int Use(Target target)
    {
        return cost;
    }
}