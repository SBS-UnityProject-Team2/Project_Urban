public class SuperConduct
{
    private readonly Target owner;
    private int remainingTurn;

    public SuperConduct(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        owner.Status.IsBlock = true;
        remainingTurn = turn;
    }

    public void Revert()
    {
        owner.Status.IsBlock = false;
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}