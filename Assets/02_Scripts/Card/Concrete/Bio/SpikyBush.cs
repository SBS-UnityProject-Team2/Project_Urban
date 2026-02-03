using System.Collections;
using UnityEngine;

public class SpikyBush : Defense
{       
    [SerializeField] private int bioActiveShellPoint = 2;
    [SerializeField] private int spikePoint = 3;
    public override CardName Name => CardName.SpikyBush;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {   
        yield return PlayEffect(target);
        target.Protect(protect);
        target.BioActiveShell(bioActiveShellPoint);
        target.Spike(spikePoint);
    }
}