using UnityEngine;

public class SurgingLife : Attack
{
    public override CardName Name => CardName.SurgingLife;

    public override int Use(Target target)
    {
        return cost;
    }
}