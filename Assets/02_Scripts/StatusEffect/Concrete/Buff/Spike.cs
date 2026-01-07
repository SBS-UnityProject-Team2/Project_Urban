public class Spike
{
    private int count;
    private bool isActive;

    public bool IsActive => isActive;

    public Spike(Target owner)
    {
        owner.OnDamaged.AddListener((attacker, target, isProtected) =>
        {
            if (isActive)
            {
                attacker.Damage(owner, count);
                isActive = false;
            }
        });
    }

    public void Active(int count)
    {
        this.count = count;
        isActive = true;
    }
}