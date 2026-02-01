using System.Collections;
using UnityEngine;

public class Pulse : Attack
{
    [SerializeField] private int bonusDamage = 6;

    public override CardName Name => CardName.Pulse;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        int finalDamage = damage;

        if (target.Status.Frozen.IsActive)
        {
            finalDamage += bonusDamage;
        }
        
        yield return PlayEffect(target);
        EnemyManager.Instance.DamageAll(finalDamage, Element.Psychic);
    }
}
