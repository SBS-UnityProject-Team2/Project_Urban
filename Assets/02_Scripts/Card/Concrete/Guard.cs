class Guard : Defense
{
    private readonly int value = 5;
    public override CardName Name => CardName.Guard;

    public override int Use(Target target)
    {
        target.ApplyStatusEffect(new Armor(value));

        return cost;
    }
}