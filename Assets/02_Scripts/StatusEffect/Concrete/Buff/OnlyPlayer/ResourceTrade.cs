public class ResourceTrade : PlayerStatusEffect
{
    public ResourceTrade(Player player) : base(player)
    {
        player.OnTurnStart.AddListener(HandleTurnStart);
    }

    public override int StatusNumber => 0;
    public override StatusEffectName Name => StatusEffectName.ResourceTrade;

    public void Active()
    {
        SetActive(true);
    }

    private void HandleTurnStart()
    {
        if (!IsActive) return;

        player.DiscardCard();

        player.Heal(1);
        player.Cost.Increase();
    }
}