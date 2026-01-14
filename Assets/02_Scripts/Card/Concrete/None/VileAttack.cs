using UnityEditor;

class VileAttack : Attack
{
    public override CardName Name => CardName.VileAttack;
    public int turn;

    public override int Use(Player player, Target target)
    {  
        target.Damage(player, damage); 
        // 디버프 약화 2턴부여
        target.Weaken(turn);

        return curCost;
    }
}