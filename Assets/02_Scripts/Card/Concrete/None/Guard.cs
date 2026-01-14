public class Guard : Defense
{
    private readonly int value = 5;
    public override CardName Name => CardName.Guard;

    public override int Use(Player player, Target target)
    {
        target.Protect(value);

        return curCost;
    }
}