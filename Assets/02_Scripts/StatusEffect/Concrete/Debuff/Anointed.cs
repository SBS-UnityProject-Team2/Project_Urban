public class Anointed : TurnStatusEffect
{
    static private readonly float damageModifier = 0.5f;

    public Anointed(Target target) : base(target)
    {
        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Anointed;

    public override int StatusNumber => remainingTurn;

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
        if (hitType != Element.Psychic) return 0;
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