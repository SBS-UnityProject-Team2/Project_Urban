using System.Collections;
using UnityEngine;

public class ElasticWall : Defense
{ 
    [SerializeField] private int turn;
    public override CardName Name => CardName.ElasticWall;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Protect(armor);
        target.BioActiveShell(turn);
        target.ElasticVeil(turn);
    }
}