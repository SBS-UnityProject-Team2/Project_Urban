public class Infested
{
    static private readonly float damageModifier = 0.5f;
    private readonly Target owner;
    private int remainingTurn;

    public Infested(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        owner.Status.IsInfested = true;
        remainingTurn = turn;
    }

    public void Revert()
    {
        owner.Status.IsInfested = false;
    }

    public int Modify(int hitPoint, Element hitType)
    {
        if (hitType != Element.Grass) return hitPoint;
        if (!owner.Status.IsInfested) return hitPoint;

        return (int)(hitPoint + hitPoint * damageModifier);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}