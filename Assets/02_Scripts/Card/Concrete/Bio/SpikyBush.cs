using System.Collections;
using UnityEngine;

public class SpikyBush : Defense
{       
    [SerializeField] private int turn;
    [SerializeField] private int count;
    public override CardName Name => CardName.SpikyBush;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {   
        yield return PlayEffect(target);
        target.Protect(armor);
        target.BioActiveShell(turn);
        target.Spike(count);
    }
}