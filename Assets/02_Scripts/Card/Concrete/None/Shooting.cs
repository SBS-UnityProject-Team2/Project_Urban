using UnityEngine;

public class Shooting : Attack
{
    public override CardName Name => CardName.Shooting;

    public override int Use(Player player, Target target)
    {
        Deck.DeckCard lastCard = player.Deck.GetLastUsedCard();
        
        Element attackElement = Element.None;

        // 카드가 있는지 확인
        if (lastCard != null)
        {
            CardDataEntry cardData = CardManager.Instance.GetCardData(lastCard.CardName);
            
            if (cardData != null)
            {
                attackElement = cardData.element;
            }
        }

        target.Damage(player, damage, attackElement);

        return curCost;
    }
}