public class Spike : ActiveStatusEffect
{
    private int count;
    public override int StatusNumber => count;

    public Spike(Target target) : base(target)
    {
        owner.OnDamaged.AddListener((attacker, target, isProtected) =>
        {
            if (!IsActive) return;

            attacker.Damage(owner, count);
            SetActive(false);
        });
    }

    public override StatusEffectName Name => StatusEffectName.Spike;

    public override void Active(int count)
    {
        this.count = count;
        SetActive(true);
    }
}