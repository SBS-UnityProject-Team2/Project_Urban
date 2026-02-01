using System.Collections;
using UnityEngine;

public class Inferno : Attack
{
    [SerializeField] private int damagePerCount = 8;

    public override CardName Name => CardName.Inferno;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        int extinctCount = user.Deck.ExtinctCardCount;
        int totalDamage = extinctCount * damagePerCount;
        
        yield return PlayEffect(target);
        target.Damage(user, totalDamage, Element.Ruin);
    }
}