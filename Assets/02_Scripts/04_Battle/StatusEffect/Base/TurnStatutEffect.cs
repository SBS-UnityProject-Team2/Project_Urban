abstract public class TurnStatusEffect : StatusEffect
{
    protected int remainingTurn;
    public override int StatusNumber => remainingTurn;

    abstract public void Apply(int turn);
    abstract public void Revert();

    protected void UpdateRemainingTurn(int turn)
    {
        remainingTurn = turn;
        NotifyStatusChanged();
    }
}