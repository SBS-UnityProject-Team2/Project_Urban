public class BioActiveShell
{
    private readonly Target owner;
    private int remainingTurn;

    public BioActiveShell(Target target)
    {
        owner = target;
    }

    public void Apply(int turn)
    {
        owner.Element = Element.Grass;
        remainingTurn = turn;
    }

    public void Revert()
    {
        if (owner.Element == Element.Grass)
            owner.Element = Element.None;
    }

    public void DecreaseTurn()
    {
        remainingTurn--;

        if (remainingTurn < 0)
            remainingTurn = 0;
    }
}