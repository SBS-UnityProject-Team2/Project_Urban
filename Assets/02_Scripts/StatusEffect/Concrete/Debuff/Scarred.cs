public class Scarred
{
    private readonly Target owner;
    private int count;
    private bool isActive;

    public Scarred(Target target)
    {
        owner = target;

        owner.OnDamaged.AddListener(HandleDamage);
    }

    public void Apply(int count)
    {
        isActive = true;
        this.count = count;
    }

    public void Revert()
    {
        isActive = false;
    }

    private void HandleDamage(Target _, Target target, bool __)
    {
        if (isActive)
            target.DebuffDamage(count);
    }
}