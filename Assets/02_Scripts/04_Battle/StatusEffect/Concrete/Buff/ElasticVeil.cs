public class ElasticVeil : ActiveStatusEffect
{
    private int count;

    public ElasticVeil(Target target) : base(target)
    {
        target.OnDamaged.AddListener(HandleDamage);
        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override int StatusNumber => count;

    public override StatusEffectName Name => StatusEffectName.ElasticVeil;

    public override void Active(int count)
    {
        SetActive(true);
        this.count = count;
    }

    private void HandleDamage(Target attacker, Target _, bool __)
    {
        if (!IsActive) return;

        attacker.Broken(count);
        attacker.Weaken(count);
    }

    private void HandleTurnEnd()
    {
        SetActive(false);
    }
}