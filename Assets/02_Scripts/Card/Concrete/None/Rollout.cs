using System;
using UnityEngine;

public class Rollout : Defense
{
    [SerializeField ]private int drawBonus;    //다음턴 드로우보너스

    public override CardName Name => CardName.Rollout;

    public override int Use(Target target)
    {
        target.Protect(armor);

        Player player = target as Player;

        player.AddNextTurnDrawCount(drawBonus);

        return cost;
    }
}