public class Burst : ActiveStatusEffect
{
    private int count;

    public Burst(Target target) : base(target)
    {
        target.OnAttack.AddListener(HandleAttack);
        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override int StatusNumber => count;

    public override StatusEffectName Name => StatusEffectName.ElasticVeil;

    public override void Active(int count)
    {
        SetActive(true);
        this.count = count;
    }

    private void HandleAttack(Target attacker, Target target)
    {
        if (!IsActive) return;

        target.DebuffDamage(count);
        SetActive(false);
    }   

    private void HandleTurnEnd()
    {
        SetActive(false);
    }
}