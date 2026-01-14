public class Dizzy : PlayerTurnStatusEffect
{
    public Dizzy(Player player) : base(player)
    {
        player.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public override int StatusNumber => remainingTurn;

    public override StatusEffectName Name => StatusEffectName.Dizzy;

    public override void Apply(int turn)
    {
        remainingTurn = turn;
        SetActive(true);
    }

    public override void Revert()
    {
        SetActive(false);
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        UpdateRemainingTurn(remainingTurn - 1);

        if (remainingTurn == 0) Revert();
    }   
}