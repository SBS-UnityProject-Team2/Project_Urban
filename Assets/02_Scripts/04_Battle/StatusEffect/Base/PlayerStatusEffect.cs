abstract public class PlayerStatusEffect : StatusEffect
{
    protected Player player;

    public PlayerStatusEffect(Player player)
    {
        this.player = player;
    }
}