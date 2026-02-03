using System.Collections;
using UnityEngine;

public class Incendiary : BuffCard
{
    [SerializeField] private int loadedIncendiaryPoint = 2;
    
    public override CardName Name => CardName.Incendiary;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.LoadedIncendiary(loadedIncendiaryPoint);
    }
}