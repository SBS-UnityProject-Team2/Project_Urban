using UnityEngine;
using System.Collections;

public class Anxiolytic : BuffCard
{
    [SerializeField] private int frozenPoint = 1;
    public override CardName Name => CardName.Anxiolytic;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        EnemyManager.Instance.ApplyAll(enemy => enemy.Frozen(frozenPoint));
    }
}