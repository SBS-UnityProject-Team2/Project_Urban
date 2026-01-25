using UnityEngine;

public class Inferno : Attack
{
    [SerializeField] private int damagePerCount = 8; // 소멸 카드 1장당 데미지

    public override CardName Name => CardName.Inferno;

    public override int Use(Player player, Target target)
    {

        // 2. 소멸된 카드 갯수 가져오기
        int extinctCount = player.CardSystem.Deck.ExtinctCardCount;

        // 3. 데미지 계산
        int totalDamage = extinctCount * damagePerCount;
        
        target.Damage(player, totalDamage, Element.Ruin);      

        return curCost;
    }
}