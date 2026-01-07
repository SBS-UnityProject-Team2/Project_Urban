public class Incendiary
{
    private int count;
    private bool isActive;

    public bool IsActive => isActive;

    public Incendiary(Target owner)
    {
        owner.OnAttack.AddListener(target =>
        {
            if (isActive)
            {
                target.Damage(owner, count);
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