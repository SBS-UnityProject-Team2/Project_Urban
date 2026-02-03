using System.Collections;
using UnityEngine;

public class DistortedSlay : Attack
{    
    public override CardName Name => CardName.DistortedSlay;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        int additionalHits = user.Deck.UsedCardCount;
        
        // 기본 1회 + 추가 횟수만큼 이펙트와 데미지 반복
        for (int i = 0; i < additionalHits + 1; i++)
        {
            yield return PlayEffect(target);
            target.Damage(user, damage, Element.Bio);
        }
    }
}