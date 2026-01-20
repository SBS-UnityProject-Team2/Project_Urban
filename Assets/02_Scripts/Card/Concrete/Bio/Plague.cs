using UnityEngine;

public class Plague : Attack
{
    [SerializeField] private int damagePerCard = 6; // 버린 카드 1장당 입힐 데미지
    [SerializeField] private int minDiscard = 0;
    [SerializeField] private int maxDiscard = 3;    // 최대 버릴 수 있는 카드 수

    public override CardName Name => CardName.Plague;

    public override int Use(Player player, Target target)
    {
        player.DiscardCard(minDiscard, maxDiscard, count =>
        {
            int totalDamage = count * damagePerCard;
            target.Damage(player, totalDamage, Element.Bio);
        });

        return curCost;
    }
}