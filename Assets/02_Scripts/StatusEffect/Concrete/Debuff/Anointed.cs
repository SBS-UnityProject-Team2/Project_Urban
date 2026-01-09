public class Anointed
{
    static private readonly float damageModifier = 0.5f;
    private readonly Target owner;
    private int remainingTurn;
    
    public Anointed(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        owner.Status.IsAnointed = true;
        remainingTurn = turn;
    }

    public void Revert()
    {
        owner.Status.IsAnointed = false;
    }

    public int Modify(int hitPoint, Element hitType)
    {
        if (hitType != Element.Flame) return hitPoint;
        if (!owner.Status.IsAnointed) return hitPoint;

        return (int)(hitPoint + hitPoint * damageModifier);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}