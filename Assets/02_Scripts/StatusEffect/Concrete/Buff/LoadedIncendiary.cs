public class LoadedIncendiary : ActiveStatusEffect
{
    private int count;

    public LoadedIncendiary(Target target) : base(target)
    {
        owner.OnAttack.AddListener(HandleAttack);
    }

    public override StatusEffectName Name => StatusEffectName.LoadedIncendiary;
    public override int StatusNumber => count;

    public override void Active(int count)
    {
        this.count = count;
        SetActive(true);
    }

    private void HandleAttack(Target attacker, Target target)
    {
        if (!IsActive) return;

        target.Damage(attacker, count, Element.Ruin);
        SetActive(false);
    }
}