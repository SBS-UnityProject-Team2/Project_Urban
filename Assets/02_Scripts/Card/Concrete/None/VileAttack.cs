using UnityEditor;

class VileAttack : Attack
{
    public override CardName Name => CardName.VileAttack;
    public int turn;

    public override int Use(Target target)
    {   
        // 디버프 약화 2턴부여
        target.Weaken(turn);

        return cost;
    }
}