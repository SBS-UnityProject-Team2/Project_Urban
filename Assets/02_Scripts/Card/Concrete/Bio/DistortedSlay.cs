using System.Collections;
using UnityEngine;

public class DistortedSlay : Attack
{    
    public override CardName Name => CardName.DistortedSlay;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        
        int additionalHits = user.Deck.UsedCardCount;
        for (int i = 0; i < additionalHits + 1; i++)
            target.Damage(user, damage, Element.Bio);
    }
}