using UnityEngine;

public class EnfeebleSludge : Attack
{
    public override CardName Name => CardName.EnfeebleSludge;

    public override int Use(Target target)
    {
        return cost;
    }
}