public class Poisoned
{
    static readonly private float hpRatio = 0.2f;
    private readonly Target owner;
    private bool isActive;

    public Poisoned(Target target)
    {
        owner = target;
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
    } 

    public void Apply()
    {
        isActive = true;
    }

    public void Revert()
    {
        isActive = false;
    }

    private void HandleTurnEnd()
    {
        if (isActive)
        {
            int damage = (int)(owner.Health.CurrentHp * hpRatio);
            owner.DebuffDamage(damage);
            isActive = false;
        }
    }
}