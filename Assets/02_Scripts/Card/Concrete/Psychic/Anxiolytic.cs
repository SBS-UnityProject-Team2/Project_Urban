using UnityEngine;

public class Anxiolytic : BuffCard
{
    public override CardName Name => CardName.Anxiolytic;

    public override int Use(Player player, Target target)
    {
        // 2. UsedCardList에서 랜덤으로 1장 뽑아오기
        Card drawnCard = player.Deck.DrawRandomFromDiscard();           
        drawnCard.SetCost(0);
    
        return curCost;
    }
}