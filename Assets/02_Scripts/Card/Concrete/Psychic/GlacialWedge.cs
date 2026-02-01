using System.Collections;
using UnityEngine;

public class GlacialWedge : Attack
{
    [SerializeField] private int turn;

    public override CardName Name => CardName.GlacialWedge;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Damage(user, damage, Element.Psychic);
        target.Frozen(turn);
    }
}