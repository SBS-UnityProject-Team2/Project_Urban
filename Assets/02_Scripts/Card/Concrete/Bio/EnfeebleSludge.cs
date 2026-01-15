using UnityEngine;
using System.Collections.Generic; 

public class EnfeebleSludge : Debuff
{
    [SerializeField] private int turn = 1;
    public override CardName Name => CardName.EnfeebleSludge;

    public override int Use(Player player, Target target)
    {
        EnemyManager.Instance.ApplyAll(enemy => enemy.Weaken(turn));
        return curCost;
    }
}