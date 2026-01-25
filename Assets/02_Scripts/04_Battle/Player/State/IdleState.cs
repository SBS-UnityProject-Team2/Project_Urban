public class IdleState : IPlayerState
{
    public void Enter(Player player)
    {
        // Idle 상태 진입 시 선택된 카드 초기화
    }

    public void Exit(Player player)
    {
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

        // 카드 선택 -> CardSelected 상태로 전환
        player.StateMachine.ChangeToCardSelected(card);
    }

    public void OnEnemyEnter(Player player, Enemy enemy)
    {
        // Idle 상태에서는 적 호버 없음
    }

    public void OnEnemyExit(Player player, Enemy enemy)
    {
        // Idle 상태에서는 적 언호버 없음
    }

    public void OnEnemyClick(Player player, Enemy enemy)
    {
        // Idle 상태에서는 적 클릭 무시
    }
}
