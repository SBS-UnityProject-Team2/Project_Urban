using System.Collections;
using UnityEngine;

public class Shooting : Attack
{
    public override CardName Name => CardName.Shooting;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        Card lastCard = user.Deck.GetLastUsedCard();
        Element attackElement = Element.None;

        if (lastCard != null)
            attackElement = lastCard.Element;

        yield return PlayEffect(target);
        target.Damage(user, damage, attackElement);
    }
}