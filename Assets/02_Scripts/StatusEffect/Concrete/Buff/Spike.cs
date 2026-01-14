public class Spike : ActiveStatusEffect
{
    private int count;
    public override int StatusNumber => count;

    public Spike(Target target) : base(target)
    {
        owner.OnDamaged.AddListener(HandleDamage);
    }

    public override StatusEffectName Name => StatusEffectName.Spike;

    public override void Active(int count)
    {
        this.count = count;
        SetActive(true);
    }

    private void HandleDamage(Target attacker, Target target, bool isProtected)
    {
        if (!IsActive) return;

        attacker.Damage(target, count);
        SetActive(false);
    }
}