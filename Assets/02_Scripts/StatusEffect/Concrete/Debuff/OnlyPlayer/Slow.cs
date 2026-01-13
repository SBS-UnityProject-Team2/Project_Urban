public class Slow : PlayerStatusEffect
{

    private int remainingTurn;

    public Slow(Player player) : base(player)
    {
        
        player.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override int StatusNumber => remainingTurn;
    public override StatusEffectName Name => StatusEffectName.Slow;

    public void Apply(int turn)
    {
        player.DecreaseDrawCount();
        remainingTurn = turn;
    }

    public void Revert()
    {
       player.IncreaseDrawCount();
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}