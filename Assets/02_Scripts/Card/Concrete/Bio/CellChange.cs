using UnityEngine;

public class CellChange : Attack
{
    public override CardName Name => CardName.CellChange;

    public override int Use(Player player, Target target)
    {   
        // 카드 선택해서 usedCardList 로 보냄
        return curCost;
    }
}