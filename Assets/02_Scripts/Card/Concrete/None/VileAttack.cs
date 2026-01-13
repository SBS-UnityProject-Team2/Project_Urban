using UnityEditor;

class VileAttack : Attack
{
    public override CardName Name => CardName.VileAttack;
    public int turn;

    public override int Use(Target target)
    {  
        Player player = BattleManager.Instance.Player;

        target.Damage(player, damage); 
        // 디버프 약화 2턴부여
        target.Weaken(turn);

        return cost;
    }
}