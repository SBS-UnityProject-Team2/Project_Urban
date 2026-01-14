class Shooting : Attack
{
    public override CardName Name => CardName.Shooting;

    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage);

        return curCost;
    }
}