public class Refined
{
    private readonly Target owner;
    private int remainingTurn;

    public Refined(Target target)
    {
        owner = target;
    }

    public void Apply(int turn)
    {
        owner.Element = Element.Flame;
        remainingTurn = turn;
    }

    public void Revert()
    {
        if (owner.Element == Element.Flame)
            owner.Element = Element.None;
    }

    public void DecreaseTurn()
    {
        remainingTurn--;

        if (remainingTurn < 0)
            remainingTurn = 0;
    }
}