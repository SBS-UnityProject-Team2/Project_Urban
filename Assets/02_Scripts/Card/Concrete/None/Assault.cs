using System.Collections;
using UnityEditor;
using UnityEngine;

public class Assault : Attack
{
    public override CardName Name => CardName.Assault;
    [SerializeField] private int turn = 1;

    protected override IEnumerator UseRoutine(Player user, Target target)
    { 
        yield return PlayEffect(target);
        target.Damage(user, damage);
        target.Frozen(turn);
    }
}