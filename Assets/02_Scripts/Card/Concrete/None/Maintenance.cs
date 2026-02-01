using System.Collections;
using UnityEngine;

public class Maintenance : BuffCard
{
    [SerializeField] private int drawCount = 2;

    public override CardName Name => CardName.Maintenance;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        user.DrawCard(drawCount);
    }
}