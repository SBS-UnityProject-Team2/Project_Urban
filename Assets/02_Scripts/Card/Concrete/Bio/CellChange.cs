using UnityEngine;
using System.Collections.Generic;

public class CellChange :BuffCard
{
    [SerializeField] private int maxDiscard = 1;
    public override CardName Name => CardName.CellChange;

    public override int Use(Player player, Target target)
    {
        // 1. Player 형변환
        Player user = player as Player;
        if (user == null) return curCost;

        // 2. 버리기 패널 호출 (최대 2장 선택)
        DiscardPanelUI.Instance.StartDiscardProcess(maxDiscard, (discardedCards) =>
        {
            // 3. 버리기 완료 후 실행될 로직            
            if (discardedCards.Count > 0)
            {
                user.DrawCard(4);
            }           
        });

        return curCost;
    }
}