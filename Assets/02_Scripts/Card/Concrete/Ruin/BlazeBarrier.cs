using System.Collections;
using UnityEngine;

public class BlazeBarrier : Defense 
{    
    [SerializeField] private int turn;
    public override CardName Name => CardName.BlazeBarrier;
    
    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Protect(armor);
        target.Refined(turn);
    }
}