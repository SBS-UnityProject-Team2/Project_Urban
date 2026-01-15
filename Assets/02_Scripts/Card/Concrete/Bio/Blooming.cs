using UnityEngine;
using System.Collections.Generic;

public class Blooming : BuffCard
{
    [SerializeField] private int costGain = 2; // 회복할 코스트 양

    public override CardName Name => CardName.Blooming;

    public override int Use(Player player, Target target)
    {
        // 1. 버리기로직 실행요청
        DiscardPanelUI.Instance.StartDiscardProcess(1, (discardedCards) =>
        {            
            // [안전장치 2] 콜백 리스트 null 체크
            if (discardedCards != null && discardedCards.Count > 0)
            {
                // 버리기 실행확인
                Player user = player as Player;

                if (user != null)
                {
                    user.Cost.Increase(costGain);
                }
            }
        });
        return curCost;
    }
}