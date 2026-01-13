public class Branded : ActiveStatusEffect
{
    private int count;
    public override int StatusNumber => count;

    public Branded(Target target) : base(target)
    {
        owner.OnDamaged.AddListener(HandleDamage);
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override StatusEffectName Name => StatusEffectName.Branded;

    public override void Active(int count)
    {
        SetActive(true);
        this.count = count;
    }

    private void HandleDamage(Target attacker, Target target, bool __)
    {
        if (IsActive)
            target.DebuffDamage(count, Element.Ruin);
    }

    private void HandleTurnEnd()
    {
        if (!IsActive) return;

        SetActive(false);
    }
}