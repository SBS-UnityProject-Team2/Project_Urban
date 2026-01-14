public class Poisoned : TurnStatusEffect
{
    static readonly private float hpRatio = 0.2f;

    public Poisoned(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Poisoned;

    public override void Apply(int turn)
    {
        remainingTurn = turn;
        SetActive(IsActive);
    }


    public override void Revert()
    {
        SetActive(true);
    }

    private void Damage()
    {
        int damage = (int)(owner.Health.CurrentHp * hpRatio);
        owner.DebuffDamage(damage);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;
        
        Damage();
        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }
}