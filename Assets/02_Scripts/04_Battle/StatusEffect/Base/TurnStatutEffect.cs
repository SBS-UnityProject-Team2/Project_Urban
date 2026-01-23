abstract public class TurnStatusEffect : StatusEffect
{
    protected readonly Target owner;
    protected int remainingTurn;
    public override int StatusNumber => remainingTurn;

    public TurnStatusEffect(Target target) : base()
    {
        owner = target;
    }

    abstract public void Apply(int turn);
    abstract public void Revert();

    protected void UpdateRemainingTurn(int turn)
    {
        remainingTurn = turn;
        NotifyStatusChanged();
    }
}