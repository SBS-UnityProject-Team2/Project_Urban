using System.Collections;
using TMPro;
using UnityEngine;

public class AbsorbingStrike : Attack
{
    public override CardName Name => CardName.AbsorbingStrike;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        int hpBefore = target.Health.CurrentHp;
        
        yield return PlayEffect(target);
        
        target.Damage(user, damage, Element.Bio);
        int hpAfter = target.Health.CurrentHp;
        int actualDamageDealt = hpBefore - hpAfter;

        if (actualDamageDealt > 0)
        {
            user.Heal(actualDamageDealt);
        }
    }
}