class Shooting : Attack
{
    public override CardName Name => CardName.Shooting;

    public override int Use(Target target)
    {
        // target.Damage(damage);

        return cost;
    }
}