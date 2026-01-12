public class Stigma
{
    private readonly Target owner;
    private int count;
    private bool isActive;

    public Stigma(Target target)
    {
        owner = target;
        owner.OnDamaged.AddListener(HandleDamage);
        owner.OnTurnEnd.AddListener(HandleTurnEnd);
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
            target.DebuffDamage(count, Element.Ruin);
    }

    private void HandleTurnEnd()
    {
        if (isActive)
            isActive = false;
    }
}