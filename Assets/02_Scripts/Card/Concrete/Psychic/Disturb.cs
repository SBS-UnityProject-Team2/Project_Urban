using System.Collections;
using UnityEngine;
using System.Collections.Generic; 

public class Disturb : Debuff
{
    [SerializeField] private int brokenPoint = 2;

    public override CardName Name => CardName.Disturb;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {   
        yield return PlayEffect(target);
        EnemyManager.Instance.ApplyAll(enemy => enemy.Broken(brokenPoint));
    }
}