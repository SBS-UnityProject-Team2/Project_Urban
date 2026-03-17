abstract public class TurnStatusEffect : StatusEffect
{
    protected readonly Actor owner;
    protected int remainingTurn;
    public override int StatusNumber => remainingTurn;

    public TurnStatusEffect(Actor target) : base()
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