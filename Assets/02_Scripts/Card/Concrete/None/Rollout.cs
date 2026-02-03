using System;
using System.Collections;
using UnityEngine;

public class Rollout : Defense
{
    [SerializeField] private int drawBonus;

    public override CardName Name => CardName.Rollout;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Protect(protect);
        user.AddNextTurnDrawCount(drawBonus);
    }
}