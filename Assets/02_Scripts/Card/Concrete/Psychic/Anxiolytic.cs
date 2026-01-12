using UnityEngine;

public class Anxiolytic : BuffCard
{
    public override CardName Name => CardName.Anxiolytic;

    public override int Use(Target target)
    {
        // 1. 플레이어 정보 가져오기
        Player player = target as Player;

        // 2. 덱(Deck)에서 버린 카드 중 하나를 랜덤으로 뽑아옴
        Card drawnCard = player.Deck.DrawRandomFromDiscard();        

        // 3. 해당 카드의 코스트를 0으로 변경
        drawnCard.SetCost(0);





        return cost;
    }
}