abstract public class PlayerTurnStatusEffect : PlayerStatusEffect
{
    protected int remainingTurn;
    public override int StatusNumber => remainingTurn;

    protected PlayerTurnStatusEffect(Player player) : base(player)
    {
    }

    abstract public void Apply(int turn);
    abstract public void Revert();

    protected void UpdateRemainingTurn(int turn)
    {
        remainingTurn = turn;
        NotifyStatusChanged();
    }
}