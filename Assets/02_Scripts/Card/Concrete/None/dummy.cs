public class Dummy : BuffCard
{

    public override CardName Name => CardName.dummy;

    public override int Use(Player player, Target _)
    {
        player.Blur(turns);
        
        return cost;
    }
}