public class Regeneration : TurnStatusEffect
{    
    private int count;

    public Regeneration(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Regeneration;

    public override void Apply(int turn)
    {
        UpdateRemainingTurn(turn);
        count = turn;
        SetActive(true);
    }

    public override void Revert()
    {
        SetActive(false);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        owner.Heal(count);
        
        UpdateRemainingTurn(remainingTurn - 1);
        count--;

        if (remainingTurn == 0) Revert();
    }
}