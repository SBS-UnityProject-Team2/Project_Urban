public class DiscardState : IPlayerState
{
    private DiscardPanelUI discardPanelUI;

    public void SetDiscardPanel(DiscardPanelUI discardPanelUI)
    {
        this.discardPanelUI = discardPanelUI;
    }

    public void Enter(Player player)
    {
        discardPanelUI.OnConfirm.AddListener(cards =>
        {
            foreach (Card card in cards)
                player.Deck.Discard(card);

            player.StateMachine.ChangeState<IdleState>();
        });
    }

    public void Exit(Player player)
    {
        discardPanelUI.OnConfirm.RemoveAllListeners();
        discardPanelUI.ClosePanel();
    }

    public void OnCardEnter(Player player, Card card)
    {
        if (!player.IsEnable()) return;

        card.Select();
    }

    public void OnCardExit(Player player, Card card)
    {
        if (!player.IsEnable()) return;

        card.UnSelect();
    }

    public void OnCardClick(Player player, Card card)
    {
        if (!player.IsEnable()) return;

        // 카드를 버리기 패널에 추가
        discardPanelUI.AddCard(card);
        card.UnSelect();
    }

    public void OnEnemyEnter(Player player, Enemy enemy)
    {
        
    }

    public void OnEnemyExit(Player player, Enemy enemy)
    {
        
    }

    public void OnEnemyClick(Player player, Enemy enemy)
    {
        
    }
}
