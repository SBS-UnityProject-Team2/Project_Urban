using UnityEngine;

public class Shooting : Attack
{
    public override CardName Name => CardName.Shooting;

    public override int Use(Player player, Target target)
    {
        // 1. Deck에서 마지막으로 낸 카드 이름 가져오기
        CardName? lastCardName = player.Deck.GetLastUsedCard();
        CardDataEntry cardData = CardManager.Instance.GetCardData(lastCardName.Value);
        Element attackElement = (lastCardName == null) ? Element.None :cardData.element;

        target.Damage(player, damage, attackElement);

        return curCost;
    }
}