using UnityEngine;
using System.Collections.Generic; 

public class Disturb : Debuff
{
    [SerializeField] private int turn; // 파갑 지속 턴 

    public override CardName Name => CardName.Disturb;

    public override int Use(Player player, Target target)
    {   
        EnemyManager.Instance.ApplyAll(enemy => enemy.Weaken(turn));

        return curCost;
    }
}