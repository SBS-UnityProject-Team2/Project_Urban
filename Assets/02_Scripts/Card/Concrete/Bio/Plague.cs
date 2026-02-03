using System.Collections;
using UnityEngine;

public class Plague : Attack
{
    [SerializeField] private int minDiscard = 0;
    [SerializeField] private int maxDiscard = 3;

    public override CardName Name => CardName.Plague;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {       
        yield return PlayEffect(target);
        user.DiscardCard(minDiscard, maxDiscard, count =>
        {
            int totalDamage = count * damage;
            target.Damage(user, totalDamage, Element.Bio);
        });
        
    }
}