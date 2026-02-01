using System.Collections;
using UnityEngine;

public class AccelConcoction : BuffCard
{
    [SerializeField] private int duration;

    public override CardName Name => CardName.AccelConcoction;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {        
        yield return PlayEffect(target);
        user.Acceleration(duration);
    }
}