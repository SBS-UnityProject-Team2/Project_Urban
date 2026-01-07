public class ElectricField
{
    private int count;
    private bool isActive;

    public bool IsActive => isActive;

    public ElectricField(Player player)
    {
        player.OnTurnEnd.AddListener(() =>
        {
            if (isActive) EnemyManager.Instance.DamageAll(count);
        });
    }

    public void Active(int count)
    {
        this.count = count;
        isActive = true;
    }
}