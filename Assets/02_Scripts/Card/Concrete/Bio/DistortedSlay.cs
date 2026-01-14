using UnityEngine;

public class DistortedSlay : Attack
{    
    public override CardName Name => CardName.DistortedSlay;

    public override int Use(Player player, Target target)
    {
        // 1. 플레이어 형변환
        Player user = player as Player;        

        // 2. 기본 공격 1회 먼저 실행
        target.Damage(user, damage, Element.Bio);

        // 3. 버려진 카드 카운트 가져오기
        int additionalHits = user.Deck.UsedCardCount;

        // 4. 그 카운트만큼 반복해서 추가 공격 실행
        for (int i = 0; i < additionalHits; i++)
        {
            target.Damage(user, damage);
        }

        return curCost;
    }
}