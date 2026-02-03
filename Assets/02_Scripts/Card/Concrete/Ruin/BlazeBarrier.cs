using System.Collections;
using UnityEngine;

public class BlazeBarrier : Defense 
{    
    [SerializeField] private int refinedPoint = 2;
    public override CardName Name => CardName.BlazeBarrier;
    
    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Protect(protect);
        target.Refined(refinedPoint);
    }
}