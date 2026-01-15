using UnityEngine;
using System.Collections.Generic;

public class Backdraft : Attack
{
    [SerializeField] private int burnCount = 5;   // 전체공격이후 부여할 화상 수치

    public override CardName Name => CardName.Backdraft;

    public override int Use(Player player, Target target)
    {
        for (int i = 0; i < target.Status.Burn.Count; i++)
            EnemyManager.Instance.DamageAll(damage, Element.Ruin);

        EnemyManager.Instance.ApplyAll(enemy => enemy.Burn(burnCount));
        player.Burn(burnCount);

        return curCost;
    }
}