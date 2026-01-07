public class Cinder
{
    private int count;
    private bool isActive;

    public bool IsActive => isActive;

    public Cinder(Player player)
    {
        player.OnDamaged.AddListener((attacker, target, isProtected) =>
        {
            if (isActive && !isProtected)
            {
                player.DrawCard(count);
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