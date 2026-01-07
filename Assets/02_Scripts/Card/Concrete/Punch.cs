class Punch : Attack
{
    public override CardName Name => CardName.Punch;

    public override int Use(Target target)
    {
        // target.Damage(damage);

        return cost;
    }
}