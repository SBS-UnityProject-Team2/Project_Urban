public class Weaken
{
    private static readonly float reductionRatio = 0.3f;
    private static int reduction;
    private readonly Target owner;
    private int remainingTurn;

    public Weaken(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        reduction = (int)(owner.Status.Attack * reductionRatio);
        owner.Status.DecreaseAttack(reduction);
        
        remainingTurn = turn;
    }

    public void Revert()
    {
        owner.Status.IncreaseAttack(reduction);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}