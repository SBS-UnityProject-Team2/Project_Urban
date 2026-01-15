using UnityEngine;

public class Plague : Attack
{
    [SerializeField] private int damagePerCard = 6; // 버린 카드 1장당 입힐 데미지
    [SerializeField] private int maxDiscard = 3;    // 최대 버릴 수 있는 카드 수

    public override CardName Name => CardName.DoubleEdge;

    public override int Use(Player player, Target target)
    {
        // 1. 버리기 UI 호출
        DiscardPanelUI.Instance.StartDiscardProcess(maxDiscard, (discardedCards) =>
        {
            // 2. 콜백: UI 패널에서 선택 및 버리기가 완료된 후 실행됨
            
            int count = discardedCards.Count; // 실제로 플레이어가 선택해서 버린 카드 수

            if (count > 0)
            {
                // 3. 총 데미지 계산 
                int totalDamage = count * damagePerCard;

                // 4. 적에게 데미지 적용
                target.Damage(player, totalDamage, Element.Bio);
            }
        });

        return curCost;
    }
}