using UnityEngine;

class VileAttack : Attack
{
    [SerializeField] private int turn;

    public override CardName Name => CardName.VileAttack;

    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage); 
        target.Weaken(turn);

        return curCost;
    }
}