using UnityEngine;

public class Plague : Attack
{
    public override CardName Name => CardName.Plague;

    public override int Use(Player player, Target target)
    {   
        // 버리기 로직, UI 구현 필요
        return curCost;
    }
}