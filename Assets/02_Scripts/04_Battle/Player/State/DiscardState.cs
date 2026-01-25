public class DiscardState : IPlayerState
{
    private DiscardPanelUI discardPanelUI;

    public void SetDiscardPanel(DiscardPanelUI discardPanelUI)
    {
        this.discardPanelUI = discardPanelUI;
    }

    public void Enter(Player player)
    {
        // 버리기 모드 진입
    }

    public void Exit(Player player)
    {
        // 버리기 모드 종료
    }

    public void OnCardEnter(Player player, Card card)
    {
        // 버리기 모드에서는 호버 처리 없음
    }

    public void OnCardExit(Player player, Card card)
    {
        // 버리기 모드에서는 언호버 처리 없음
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
        // 버리기 모드에서는 적 호버 무시
    }

    public void OnEnemyExit(Player player, Enemy enemy)
    {
        // 버리기 모드에서는 적 언호버 무시
    }

    public void OnEnemyClick(Player player, Enemy enemy)
    {
        // 버리기 모드에서는 적 클릭 무시
    }
}
