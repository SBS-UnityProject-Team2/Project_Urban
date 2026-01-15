using UnityEngine;

public class DoubleEdge : Attack
{    [SerializeField] private int selfDamage;   // 반동 데미지

    public override CardName Name => CardName.DoubleEdge;

    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage, Element.Bio);
        player.Damage(player, selfDamage, Element.Bio);

        return curCost;
    }
}