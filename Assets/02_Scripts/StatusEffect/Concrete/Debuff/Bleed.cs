public class Bleed : StatusEffect
{
    private readonly Target owner;
    private int bleedPoint;

    public override StatusEffectName Name => StatusEffectName.Bleed;
    public override int StatusNumber => bleedPoint;

    public Bleed(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    public void Increase(int amount)
    {
        bleedPoint += amount;
        SetActive(true);
    }

    public int Decrease(int amount)
    {
        bleedPoint -= amount;

        if (bleedPoint <= 0)
        {
            bleedPoint = 0;
            SetActive(true);

            return 0;
        }

        NotifyStatusChanged();
        return bleedPoint;
    }

    private void HandleTurnEnd()
    {
        if (!IsActive) return;

        owner.DebuffDamage(bleedPoint);
    }
}