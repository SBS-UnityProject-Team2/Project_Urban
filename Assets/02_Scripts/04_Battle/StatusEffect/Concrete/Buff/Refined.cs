public class Refined : TurnStatusEffect
{
    public Refined(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Refined;

    public override void Apply(int turn)
    {
        owner.Element = Element.Ruin;
        UpdateRemainingTurn(turn);
        SetActive(true);
    }

    public override void Revert()
    {
        if (owner.Element == Element.Ruin)
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