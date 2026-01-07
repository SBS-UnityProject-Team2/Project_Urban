using UnityEngine;

public class IceShield : Defense
{
    public override CardName Name => CardName.IceShield;

    public override int Use(Target target)
    {
        // 1. 방어도 추가
        target.Protect(armor);
        target.Element = Element.Ice;
        
        return cost;
    }
}