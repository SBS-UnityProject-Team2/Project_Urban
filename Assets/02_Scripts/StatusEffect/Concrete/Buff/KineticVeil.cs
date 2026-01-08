public class KineticVeil
{
    private readonly Target owner;
    private int remainingTurn;

    public KineticVeil(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        owner.Element = Element.Psychic ;
        remainingTurn = turn;
    }

    public void Revert()
    {
        if (owner.Element == Element.Psychic)
            owner.Element = Element.None;
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}