public class Burst : ActiveStatusEffect
{
    private int count;

    public Burst(Target target) : base(target)
    {
        target.OnAttack.AddListener(HandleAttack);
        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override int StatusNumber => count;
    private int waitTurn = 0;

    public override StatusEffectName Name => StatusEffectName.Burst;

    public override void Active(int count)
    {
        this.count = count;
        waitTurn = 1;
        SetActive(true);
    }

    private void HandleAttack(Target attacker, Target target)
    {
        if (!IsActive) return;

        target.DebuffDamage(count);
        SetActive(false);
    }

    private void HandleTurnEnd()
    {
        if (IsActive)
        {
            if (waitTurn == 0)
                SetActive(false);

            waitTurn--;
        }
    }
}