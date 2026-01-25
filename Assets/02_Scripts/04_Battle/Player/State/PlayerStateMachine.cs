using System.Collections.Generic;

public class PlayerStateMachine
{
    private Player player;
    private IPlayerState currentState;
    
    // 미리 생성된 상태 객체들
    private readonly Dictionary<System.Type, IPlayerState> states = new();
    
    public IPlayerState CurrentState => currentState;

    public PlayerStateMachine(Player player)
    {
        this.player = player;
        
        states[typeof(IdleState)] = new IdleState();
        states[typeof(CardSelectedState)] = new CardSelectedState();
        states[typeof(DiscardState)] = new DiscardState();
    }

    public void ChangeState<T>() where T : IPlayerState
    {
        if (states.TryGetValue(typeof(T), out IPlayerState newState))
        {
            currentState?.Exit(player);
            currentState = newState;
            currentState.Enter(player);
        }
    }

    public void ChangeToCardSelected(Card card)
    {
        if (states.TryGetValue(typeof(CardSelectedState), out IPlayerState state))
        {
            if (state is CardSelectedState cardSelectedState)
            {
                currentState?.Exit(player);
                cardSelectedState.SetCard(card);
                currentState = cardSelectedState;
                currentState.Enter(player);
            }
        }
    }

    public void ChangeToDiscard(DiscardPanelUI discardPanelUI)
    {
        if (states.TryGetValue(typeof(DiscardState), out IPlayerState state))
        {
            if (state is DiscardState discardState)
            {
                currentState?.Exit(player);
                discardState.SetDiscardPanel(discardPanelUI);
                currentState = discardState;
                currentState.Enter(player);
            }
        }
    }

    public bool IsIdle() => currentState is IdleState;
    public bool IsCardSelected() => currentState is CardSelectedState;
    public bool IsDiscarding() => currentState is DiscardState;
}
