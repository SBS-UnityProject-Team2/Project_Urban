class VileAttack : Attack
{
    public override CardName Name => CardName.VileAttack;

    public override int Use(Target target)
    {
        // target.Damage(damage);

        return cost;
    }
}