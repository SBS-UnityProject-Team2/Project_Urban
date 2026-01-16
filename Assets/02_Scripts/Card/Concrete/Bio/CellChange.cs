public class CellChange : BuffCard
{
    public override CardName Name => CardName.CellChange;

    public override int Use(Player player, Target target)
    {
        Player user = player as Player;
        if (user == null) return curCost;
        
        DiscardPanelUI.Instance.StartDiscardProcess(2, (discardedCards) =>
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