public class Bleed
{
    private readonly Target owner;
    private int bleedPoint;

    public Bleed(Target target)
    {
        this.owner = target;

        target.OnTurnEnd.AddListener(() => {
            // owner.Damage(bleedPoint);
        });
    }

    public void Increase(int amount)
    {
        bleedPoint += amount;
    }     

    public int Decrease(int amount)
    {
        bleedPoint -= amount;

        if (bleedPoint < 0)
            bleedPoint = 0;

        return bleedPoint;
    }
}