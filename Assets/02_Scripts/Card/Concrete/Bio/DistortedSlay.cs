using UnityEngine;

public class DistortedSlay : Attack
{    
    public override CardName Name => CardName.DistortedSlay;

    public override int Use(Player player, Target target)
    {
        int additionalHits = player.Deck.UsedCardCount;

        // 기본 타수 1회 추가
        for (int i = 0; i < additionalHits + 1; i++)
            target.Damage(player, damage, Element.Bio);
    
        return curCost;
    }
}