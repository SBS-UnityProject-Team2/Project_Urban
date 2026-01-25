public interface IPlayerState
{
    void Enter(Player player);
    void Exit(Player player);
    
    // Card Event Handlers
    void OnCardEnter(Player player, Card card);
    void OnCardExit(Player player, Card card);
    void OnCardClick(Player player, Card card);
    
    // Enemy Event Handlers
    void OnEnemyEnter(Player player, Enemy enemy);
    void OnEnemyExit(Player player, Enemy enemy);
    void OnEnemyClick(Player player, Enemy enemy);
}
