using System.Collections;
using UnityEngine;

public class DoubleEdge : Attack
{    
    [SerializeField] private int selfDamage;

    public override CardName Name => CardName.DoubleEdge;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Damage(user, damage, Element.Bio);
        user.Damage(user, selfDamage, Element.Bio);
    }
}