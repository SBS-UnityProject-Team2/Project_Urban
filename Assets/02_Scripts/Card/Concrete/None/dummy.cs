public class Dummy : BuffCard
{
    public override CardName Name => CardName.Dummy;

    public override int Use(Player player, Target target)
    {
        target.Blur(turns);
        
        return curCost;
    }
}