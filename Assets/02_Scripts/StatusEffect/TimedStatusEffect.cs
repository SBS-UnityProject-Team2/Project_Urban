abstract public class TimedStatusEffect : StatusEffect
{
    private readonly int initTurn;
    private int turn;
    public int RemainingTurn => turn;

    public TimedStatusEffect(int initTurn)
    {
        this.initTurn = initTurn;
    }

    public void DecreaseTurn(int amount = 1)
    {
        turn -= amount;

        if (turn < 0)
            turn = 0;
    }

    public void IncreaseTurn(int amount = 1)
    {
        turn += amount;
    }

    public void RefreshTurn()
    {
        turn = initTurn;   
    }

    abstract public void Revert(Target target);
}