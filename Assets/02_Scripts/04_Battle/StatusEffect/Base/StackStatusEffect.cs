abstract public class StackStatusEffect : StatusEffect
{
    protected int stack;

    protected StackStatusEffect(Actor owner) : base(owner)
    {
    }

    public int Stack => stack;
    public override int StatusNumber => stack;
    
    public void IncreaseStack(int count = 1)
    {
        stack += count;
        SetActive(true);
    }

    public void DecreaseStack(int count = 1)
    {
        stack -= count;

        if (stack <= 0)
        {
            stack = 0;
            SetActive(false);

            return;
        }

        NotifyStatusChanged();
    }
}