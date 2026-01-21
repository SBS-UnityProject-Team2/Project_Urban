public class Regeneration : TurnStatusEffect
{    

    public Regeneration(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Regeneration;

    public override void Apply(int turn)
    {
        remainingTurn = turn;
        SetActive(true);
    }

    public override void Revert()
    {
        SetActive(false);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        owner.Heal(remainingTurn);
        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }
}