using UnityEditor;
using UnityEngine;

public class Assault : Attack
{
    public override CardName Name => CardName.Assault;
    [SerializeField] private int turn = 1;

    public override int Use(Player player, Target target)
    {   
        target.Damage(player, damage); 
        // 디버프 동결 2턴부여
        target.Frozen(turn);

        return curCost;
    }
}