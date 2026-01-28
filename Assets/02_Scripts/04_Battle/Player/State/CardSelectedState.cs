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

        if (selectedCard == card)
        {
            TryUseCardOnSelf(player, card);
            return;
        }

        selectedCard.UnSelect();
        selectedCard = card;
        selectedCard.Select();
    }

    public void OnEnemyEnter(Player player, Enemy enemy)
    {
        if (!player.IsEnable()) return;

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

        if (selectedCard.Cost > player.Cost.CurrentCost)
        {
            selectedCard.UnSelect();
            player.StateMachine.ChangeState<IdleState>();

            return;
        }

        player.UseCard(selectedCard, enemy);
        player.StateMachine.ChangeState<IdleState>();
    }

    private void TryUseCardOnSelf(Player player, Card card)
    {
        if (card.Type != CardType.Defense && card.Type != CardType.BuffCard)
            return;

        if (card.Cost > player.Cost.CurrentCost)
        {
            selectedCard.UnSelect();
            player.StateMachine.ChangeState<IdleState>();

            return;
        }

        player.UseCard(card, player);
        player.StateMachine.ChangeState<IdleState>();
    }
}
