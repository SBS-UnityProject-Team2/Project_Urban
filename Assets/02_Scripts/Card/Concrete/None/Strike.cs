class Strike : Attack
{
    public override CardName Name => CardName.Strike;

    public override int Use(Target target)
    {
        // target.Damage(damage);

        return cost;
    }
}