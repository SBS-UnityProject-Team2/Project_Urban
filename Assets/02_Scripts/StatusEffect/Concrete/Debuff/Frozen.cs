public class Frozen : TurnStatusEffect
{

    public Frozen(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }
    public override StatusEffectName Name => StatusEffectName.Frozen;
    
    public override void Apply(int turn)
    {
        SetActive(true);
        UpdateRemainingTurn(turn);
    }

    public override void Revert()
    {
        SetActive(false);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }
}