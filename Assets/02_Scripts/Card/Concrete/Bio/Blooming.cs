using UnityEngine;

public class Blooming : BuffCard
{
    [SerializeField] private int costGain = 2; // 회복 코스트

    public override CardName Name => CardName.Blooming;

    public override int Use(Player player, Target target)
    {
        // 1. UI 패널에게 카드 선택 요청
        DiscardPanelUI.Instance.StartSelectionProcess((selectedCard) =>
        {
            // 카드 버리기 (Deck 기능 호출)
            player.Deck.Discard(selectedCard);

            // 코스트 회복
            player.Cost.Increase(costGain);
        });
        return cost;
    }
}