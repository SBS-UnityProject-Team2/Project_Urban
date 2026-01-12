public class Burn 
{
    private readonly Target owner;
    private int remainingTurn;
    private int count;
    public int CurrentCount => count;   // 외부에서 현재 화상수치 가져가는용도

    public Burn(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Apply(int turn)
    {
        remainingTurn = turn;
        count = turn;
    }

    public void Revert()
    {
        remainingTurn = 0;
        count = 0;
    }

    private void HandleTurnEnd()
    {
        if (remainingTurn == 0) return;

        owner.DebuffDamage(count);
        
        remainingTurn--;
        count--;

        if (remainingTurn == 0) Revert();
    }

    
}