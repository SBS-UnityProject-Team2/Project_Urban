using UnityEngine;

public class CardSelectedState : IPlayerState
{
    private Card selectedCard;

    public void SetCard(Card card)
    {
        selectedCard = card;
    }

    public void Enter(Player player)
    {
        if (selectedCard != null)
            selectedCard.Select();
    }

    public void Exit(Player player)
    {
        if (selectedCard != null)
        {
            selectedCard.UnSelect();
            selectedCard = null;
        }
    }

    public void OnCardEnter(Player player, Card card)
    {
        // 카드가 이미 선택된 상태에서는 호버 무시
    }

    public void OnCardExit(Player player, Card card)
    {
        // 카드가 이미 선택된 상태에서는 언호버 무시
    }

    public void OnCardClick(Player player, Card card)
    {
        if (!player.IsEnable()) return;

        // 같은 카드를 다시 클릭 -> 자신에게 사용 시도
        if (selectedCard == card)
        {
            TryUseCardOnSelf(player, card);
            return;
        }

        // 다른 카드 클릭 -> 카드 교체
        selectedCard.UnSelect();
        selectedCard = card;
        selectedCard.Select();
    }

    public void OnEnemyEnter(Player player, Enemy enemy)
    {
        if (!player.IsEnable()) return;

        // 공격/디버프 카드일 때만 적 호버
        if (selectedCard.Type == CardType.Attack || selectedCard.Type == CardType.Debuff)
        {
            enemy.Hover();
        }
    }

    public void OnEnemyExit(Player player, Enemy enemy)
    {
        if (!player.IsEnable()) return;
        
        enemy.UnHover();
    }

    public void OnEnemyClick(Player player, Enemy enemy)
    {
        if (!player.IsEnable()) return;

        // 공격/디버프 카드가 아니면 무시
        if (selectedCard.Type != CardType.Attack && selectedCard.Type != CardType.Debuff)
            return;

        // 코스트 부족 체크
        if (selectedCard.Cost > player.Cost.CurrentCost)
        {
            Debug.Log($"코스트 부족: {selectedCard.Cost}/{player.Cost.CurrentCost}");
            return;
        }

        // 카드 사용
        player.UseCard(selectedCard, enemy);
        
        // Idle 상태로 복귀
        player.StateMachine.ChangeState<IdleState>();
    }

    private void TryUseCardOnSelf(Player player, Card card)
    {
        // 방어/버프 카드가 아니면 무시
        if (card.Type != CardType.Defense && card.Type != CardType.BuffCard)
            return;

        // 코스트 부족 체크
        if (card.Cost > player.Cost.CurrentCost)
        {
            Debug.Log($"코스트 부족: {card.Cost}/{player.Cost.CurrentCost}");
            return;
        }

        // 카드 사용
        player.UseCard(card, player);
        
        // Idle 상태로 복귀
        player.StateMachine.ChangeState<IdleState>();
    }
}
