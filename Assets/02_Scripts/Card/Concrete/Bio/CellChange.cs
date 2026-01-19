using UnityEngine;
using System.Collections.Generic;
public class CellChange : BuffCard
{
    [SerializeField] private int targetDiscardCount = 2; 

    public override CardName Name => CardName.CellChange;

    public override int Use(Player player, Target target)
    {
        Player user = player as Player;
        if (user == null) return curCost;
        DiscardPanelUI.Instance.StartDiscardProcess(targetDiscardCount, (discardedCards) =>
        {
            if (discardedCards.Count > 0)
            {
                user.DrawCard(3);
            }           
        }, targetDiscardCount); 

        return curCost;
    }
}