public class Scarred : ActiveStatusEffect
{
    private int count;
    public override int StatusNumber => count;

    public Scarred(Target target) : base(target)
    {
        owner.OnDamaged.AddListener(HandleDamage);
    }

    public override StatusEffectName Name => StatusEffectName.Scarred;

    public override void Active(int count)
    {
        SetActive(true);
        this.count = count;
    }

    private void HandleDamage(Target _, Target target, bool __)
    {
        if (!IsActive) return;

        target.DebuffDamage(count);
    }
}