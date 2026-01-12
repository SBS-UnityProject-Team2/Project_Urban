using UnityEngine;

public class dummy : BuffCard
{

    public override CardName Name => CardName.dummy;

    public override int Use(Target target)
    {
        target.Dummy(turns);
        
        return cost;
    }
}