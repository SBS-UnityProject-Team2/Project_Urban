public class Weaken : TurnStatusEffect
{
    private static readonly float reductionRatio = 0.3f;
    private int reduction = 0;

    public int Reduction => reduction;

    public Weaken(Target target) : base(target)
    {
        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Weaken;

    public override void Apply(int turn)
    {
        reduction = (int)(owner.Status.Attack * reductionRatio);
        remainingTurn = turn;
        SetActive(true);
    }

    public override void Revert()
    {
        reduction = 0;
        SetActive(false);
    }
    
    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }
}