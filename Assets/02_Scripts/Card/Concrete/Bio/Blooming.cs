using UnityEngine;
using System.Collections.Generic;

public class Blooming : BuffCard
{
    [SerializeField] private int costGain = 2; // 회복할 코스트 양
    [SerializeField] private int maxDiscard = 1;

    public override CardName Name => CardName.Blooming;

    public override int Use(Player player, Target target)
    {
        // 1. 버리기로직 실행요청
        DiscardPanelUI.Instance.StartDiscardProcess(maxDiscard, (discardedCards) =>
        {             
            // 2. 버리기로직 실행완료후 콜백
            if (discardedCards.Count > 0)
                player.Cost.Increase(costGain);
        });
        return curCost;
    }
}