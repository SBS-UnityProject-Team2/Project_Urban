public class Searing : PlayerStatusEffect
{
    private int count;

    public Searing(Player player) : base(player)
    {
        player.OnDamaged.AddListener(HandleDamage);
    }

    public override int StatusNumber => count;
    public override StatusEffectName Name => StatusEffectName.Searing;

    public void Active(int count)
    {
        this.count = count;
        SetActive(true);
    }

    public void HandleDamage(Target attacker, Target target, bool isProtected)
    {
        if (isProtected) return;
        if (!IsActive) return;

        player.DrawCard(count);
        SetActive(true);
    }
}