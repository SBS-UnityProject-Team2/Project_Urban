using UnityEngine;
using System.Collections.Generic; 

public class CryoPowder : Debuff
{
    [SerializeField] private int turn = 1; // 빙결 지속 턴 

    public override CardName Name => CardName.CryoPowder;

    public override int Use(Player player, Target target)
    {
        EnemyManager.Instance.ApplyAll(enemy => enemy.Frozen(turn));
        return curCost;
    }
}