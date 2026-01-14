using System;
using UnityEngine;

public class Rollout : Defense
{
    [SerializeField ]private int drawBonus;    //다음턴 드로우보너스

    public override CardName Name => CardName.Rollout;

    public override int Use(Player player, Target target)
    {
        target.Protect(armor);
        Player user = player as Player;       
        user.AddNextTurnDrawCount(drawBonus);     // 다음1턴 드로우보너스      

        return curCost;
    }
}