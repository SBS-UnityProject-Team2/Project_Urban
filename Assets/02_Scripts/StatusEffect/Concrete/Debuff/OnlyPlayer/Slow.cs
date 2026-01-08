public class Slow
{
    private readonly Player player;
    private int remainingTurn;

    public Slow(Player player)
    {
        this.player = player;

        player.OnTurnEnd.AddListener(HandleTurnEnd);
    }

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