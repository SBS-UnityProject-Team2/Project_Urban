class Strike : Attack
{
    public override CardName Name => CardName.Strike;

    public override int Use(Target target)
    {
        Player player = BattleManager.Instance.Player;

        target.Damage(player, damage);

        return cost;
    }
}