public class AccelConcoction
{
    private readonly Player player;
    private int remainingTurn;

    public AccelConcoction(Player player)
    {
        this.player = player;

        player.OnTurnStart.AddListener(HandleTurnStart);
    }

    public void Apply(int turn)
    {
        remainingTurn = turn;
    }

    public void Revert()
    {
    }

    private void HandleTurnStart()
    {
        if (remainingTurn == 0) return;

        player.DrawCard();
        player.Cost.Increase();

        remainingTurn--;

        if (remainingTurn == 0) Revert();
    }
}