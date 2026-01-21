public class Broken : TurnStatusEffect
{
    private readonly float damageModifier = 0.3f;    

    public Broken(Target target) : base(target)
    {
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }
    
    public override StatusEffectName Name => StatusEffectName.Broken;

    public override void Apply(int turn)
    {
        remainingTurn = turn;
        SetActive(true);
    }

    public override void Revert()
    {
        SetActive(false);
    }

    public int Modify(int damage)
    {   
        if (!IsActive) return 0;

        return (int)(-damage * damageModifier);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }
}