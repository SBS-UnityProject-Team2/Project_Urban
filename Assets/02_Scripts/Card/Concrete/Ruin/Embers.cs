public class Embers : Attack
{ 
    public override CardName Name => CardName.Embers;

    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage, Element.Ruin);
        player.Deck.Copy(this);
        
        return curCost;
    }
}