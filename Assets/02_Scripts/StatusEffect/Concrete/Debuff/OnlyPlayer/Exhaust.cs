public class Exhaust
{
    private readonly Player player;
    private int remainingTurn;

    public Exhaust(Player player)
    {
        player.OnTurnEnd.AddListener(HandleTurnEnd);
    }

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