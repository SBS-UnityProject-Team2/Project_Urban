using System.Collections;
using UnityEngine;

public class Cinder : BuffCard 
{   
    [SerializeField] private int searingPoint = 1;
    public override CardName Name => CardName.Cinder;
    
    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Searing(searingPoint);
    }
}
