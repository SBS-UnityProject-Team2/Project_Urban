using UnityEngine;

public class BlazeBarrier : Defense 
{    
    [SerializeField] private int turn;
    public override CardName Name => CardName.BlazeBarrier;
    public override int Use(Target target)
    {
        target.Protect(armor);
        target.Refined(turn);

        return cost;
    }
}