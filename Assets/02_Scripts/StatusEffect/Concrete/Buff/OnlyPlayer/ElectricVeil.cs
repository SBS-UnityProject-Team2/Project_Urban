public class ElectricVeil : PlayerStatusEffect
{
    private int count;

    public ElectricVeil(Player player) : base(player)
    {
        player.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override int StatusNumber => count;

    public override StatusEffectName Name => StatusEffectName.ElectricVeil;

    public void Active(int count)
    {
        this.count = count;
        SetActive(true);
    }

    private void HandleTurnEnd()
    {
        if (!IsActive) return; 
        
        EnemyManager.Instance.DamageAll(count, Element.Psychic);
    }
}