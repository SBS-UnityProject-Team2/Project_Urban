using UnityEngine;

public class Shooting : Attack
{
    public override CardName Name => CardName.Shooting;

    public override int Use(Player player, Target target)
    {
        Card lastCard = player.CardSystem.Deck.GetLastUsedCard();
        
        Element attackElement = Element.None;

        // 카드가 있는지 확인
        if (lastCard != null)
            attackElement = lastCard.Element;

        target.Damage(player, damage, attackElement);

        return curCost;
    }
}