public class Delirium
{
    static private readonly float damageModifier = 0.5f;
    private readonly Target owner;
    private int remainingTurn;
    
    public Delirium(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        owner.Status.IsDelirium = true;
        remainingTurn = turn;
    }

    public void Revert()
    {
        owner.Status.IsDelirium = false;
    }

    public int Modify(int hitPoint, Element hitType)
    {
        if (hitType != Element.Ice) return hitPoint;
        if (!owner.Status.IsDelirium) return hitPoint;

        return (int)(hitPoint + hitPoint * damageModifier);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}