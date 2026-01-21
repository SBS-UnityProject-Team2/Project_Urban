public class Burn : TurnStatusEffect
{    
    public int Count => remainingTurn;   // 외부에서 현재 화상수치 가져가는용도

    public Burn(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Burn;

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

        owner.DebuffDamage(remainingTurn);
        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }

    
}