public class Infested :  TurnStatusEffect
{
    static private readonly float damageModifier = 0.5f;

    public Infested(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Infested;

    public override void Apply(int turn)
    {
        remainingTurn = turn;
        SetActive(true);
    }

    public override void Revert()
    {
        SetActive(false);
    }

    public int Modify(int hitPoint, Element hitType)
    {
        if (hitType != Element.Bio) return 0;
        if (!IsActive) return 0;

        return (int)(hitPoint * damageModifier);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }
}