public class Exhaust : PlayerStatusEffect
{
    private int remainingTurn;

    public Exhaust(Player player) : base(player)
    {
        player.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override int StatusNumber => remainingTurn;
    public override StatusEffectName Name => StatusEffectName.Exhaust;

    public void Apply(int turn)
    {
        player.Cost.DecreaseRecovery();
        remainingTurn = turn;
    }

    public void Revert()
    {
        player.Cost.IncreaseRecovery();
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}