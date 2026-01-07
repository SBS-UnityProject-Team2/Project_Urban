public class AttackStatus : Status
{
    private int attackPoint;

    public void Increase(int amount = 1)
    {
        attackPoint += amount;
    }
}