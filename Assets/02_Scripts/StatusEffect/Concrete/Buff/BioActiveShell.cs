public class BioActiveShell : TurnStatusEffect
{
    public BioActiveShell(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.BioActiveShell;

    public override void Apply(int turn)
    {
        owner.Element = Element.Bio;
        UpdateRemainingTurn(turn);
        SetActive(true);
    }

    public override void Revert()
    {
        if (owner.Element == Element.Bio)
            owner.Element = Element.None;

        SetActive(false);
    }
    
    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }
}