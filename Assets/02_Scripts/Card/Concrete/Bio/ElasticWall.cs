using System.Collections;
using UnityEngine;

public class ElasticWall : Defense
{ 
    [SerializeField] private int bioActiveShellPoint = 2;
    [SerializeField] private int elasticVeilPoint = 2;
    public override CardName Name => CardName.ElasticWall;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Protect(protect);
        target.BioActiveShell(bioActiveShellPoint);
        target.ElasticVeil(elasticVeilPoint);
    }
}