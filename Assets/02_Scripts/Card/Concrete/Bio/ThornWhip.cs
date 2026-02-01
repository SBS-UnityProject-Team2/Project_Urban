using System.Collections;
using UnityEngine;

public class ThornWhip : Attack 
{   
    [SerializeField] private int turn;
    public override CardName Name => CardName.ThornWhip;

    protected override IEnumerator UseRoutine(Player user, Target target)
    { 
        yield return PlayEffect(target);
        target.Damage(user, damage, Element.Bio);
        target.Broken(turn);
    }
}