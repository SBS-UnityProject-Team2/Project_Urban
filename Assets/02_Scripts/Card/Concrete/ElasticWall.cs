using UnityEngine;

public class ElasticWall : Defense
{
    public override CardName Name => CardName.ElasticWall;

    public override int Use(Target target)
    {
        target.Protect(armor);
        target.Element = Element.Psychic;

        return cost;
    }
}