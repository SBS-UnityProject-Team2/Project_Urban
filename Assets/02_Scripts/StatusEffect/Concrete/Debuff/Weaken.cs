public class Weaken
{
    private static readonly float ratio = 0.3f;
    private readonly Target owner;
    private int remainingTurn;

    public Weaken(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        int attack = (int)(owner.Status.Attack * ratio);
        owner.Status.DecreaseAttack(attack);
        
        remainingTurn = turn;
    }

    public void Revert()
    {
        if (owner.Element == Element.Ice)
            owner.Element = Element.None;
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}