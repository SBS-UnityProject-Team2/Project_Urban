public class Acceleration : PlayerTurnStatusEffect
{

    public Acceleration(Player player) : base(player)
    {
        player.OnTurnStart.AddListener(HandleTurnStart);
    }

    public override int StatusNumber => remainingTurn;

    public override StatusEffectName Name => StatusEffectName.Acceleration;

    public override void Apply(int turn)
    {
        remainingTurn = turn;
        SetActive(true);
    }

    public override void Revert()
    {
        SetActive(false);
    }

    private void HandleTurnStart()
    {
        if (remainingTurn == 0) return;

        player.DrawCard();
        player.Cost.Increase();

        UpdateRemainingTurn(--remainingTurn);

        if (remainingTurn == 0) Revert();
    }
}