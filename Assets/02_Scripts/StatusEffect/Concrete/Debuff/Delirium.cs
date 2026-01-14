public class Delirium : TurnStatusEffect
{
    static private readonly float damageModifier = 0.5f;
    

    public Delirium(Target target) : base(target)
    {
        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Delirium;
    

    public override void Apply(int turn)
    {
        SetActive(true);
        UpdateRemainingTurn(turn);
    }

    public override void Revert()
    {
        SetActive(false);
    }

    public int Modify(int hitPoint, Element hitType)
    {
        if (hitType != Element.Ruin) return 0;
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