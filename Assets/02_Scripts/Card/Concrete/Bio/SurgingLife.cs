using System.Collections;
using UnityEngine;

public class SurgingLife : BuffCard
{
    [SerializeField] private int count = 5;
    public override CardName Name => CardName.SurgingLife;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Regeneration(count);
    }
}