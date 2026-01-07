using UnityEngine;

public class ElasticBarrier : Defense
{
    public override CardName Name => CardName.ElasticBarrier;

    public override int Use(Target target)
    {
        target.Protect(armor);
        target.Element = Element.Ice;

        return cost;
    }
}