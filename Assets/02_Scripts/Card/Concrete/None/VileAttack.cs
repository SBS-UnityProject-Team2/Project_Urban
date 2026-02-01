using System.Collections;
using UnityEngine;

class VileAttack : Attack
{
    [SerializeField] private int turn;

    public override CardName Name => CardName.VileAttack;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Damage(user, damage);
        target.Weaken(turn);
    }
}