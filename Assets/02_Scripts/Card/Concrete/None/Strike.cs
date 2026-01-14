class Strike : Attack
{
    public override CardName Name => CardName.Strike;

    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage);

        return cost;
    }
}