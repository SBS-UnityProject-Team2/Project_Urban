using UnityEngine;

public class Embers : Attack
{ 
    public override CardName Name => CardName.Embers;

    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage, Element.Ruin);

        // 본인카드를 복사해서 덱에 추가
        player.Deck.AddCardToDrawPile(Name);
        
        return curCost;
    }
}