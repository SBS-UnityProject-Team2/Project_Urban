using UnityEngine;
using System.Collections.Generic;
public class CellChange : BuffCard
{
    [SerializeField] private int maxDiscard = 1;
    public override CardName Name => CardName.CellChange;

    public override int Use(Player player, Target target)
    {
        Player user = player as Player;
        if (user == null) return curCost;

        // 2. 버리기 패널 호출 (최대 2장 선택)
        DiscardPanelUI.Instance.StartDiscardProcess(maxDiscard, (discardedCards) =>
        {
            // 2장이 선택되었을 때만
            if (discardedCards.Count > 0)
            {
                user.DrawCard(3);
            }           
        }, 2); // 최소 2장

        return curCost;
    }
}