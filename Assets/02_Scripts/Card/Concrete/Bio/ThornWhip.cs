using UnityEngine;

public class ThornWhip : Attack 
{   
    [SerializeField] private int turn;
    public override CardName Name => CardName.ThornWhip;

    public override int Use(Target target)
    {   
        // 1. 플레이어 가져오기
        Player player = BattleManager.Instance.Player;

        // 2. 적에게 데미지 입히기
        target.Damage(player, damage);

        target.Broken(turn);
        return cost;
    }
}