public class Regeneration 
{
    private readonly Target owner;
    private int remainingTurn;
    private int count;

    public Regeneration(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        remainingTurn = turn;
        count = turn;
    }

    public void Revert()
    {
        
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        owner.Heal(count);
        
        remainingTurn--;
        count--;

        if (remainingTurn == 0) Revert();
    }
}