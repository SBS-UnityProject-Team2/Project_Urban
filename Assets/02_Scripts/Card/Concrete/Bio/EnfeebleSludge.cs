using System.Collections;
using UnityEngine;
using System.Collections.Generic; 

public class EnfeebleSludge : Debuff
{
    [SerializeField] private int weakenPoint = 1;
    public override CardName Name => CardName.EnfeebleSludge;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        EnemyManager.Instance.ApplyAll(enemy => enemy.Weaken(weakenPoint));
    }
}