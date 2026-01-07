using UnityEngine;

public class BlazeBarrier : Defense 
{    
    public override CardName Name => CardName.BlazeBarrier;
    public override int Use(Target target)
    {
        target.Protect(armor);
        target.Element = Element.Flame;

        return cost;
    }
}