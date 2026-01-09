using System.Runtime.CompilerServices;

public class Frozen 
{
    private readonly Target owner;
    private int remainingTurn;
    
    public Frozen(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        owner.Status.IsFrozen = true;
        remainingTurn = turn;
    }

    public void Revert()
    {
        owner.Status.IsFrozen = false;
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}