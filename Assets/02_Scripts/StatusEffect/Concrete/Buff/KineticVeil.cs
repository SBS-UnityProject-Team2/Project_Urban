public class KineticVeil : TurnStatusEffect
{
    public KineticVeil(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.KineticVeil;

    public override void Apply(int turn)
    {
        owner.Element = Element.Psychic ;
        UpdateRemainingTurn(turn);
        SetActive(true);
    }

    public override void Revert()
    {
        if (owner.Element == Element.Psychic)
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