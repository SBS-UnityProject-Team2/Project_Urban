abstract public class ActiveStatusEffect : StatusEffect
{
    protected readonly Target owner;

    public ActiveStatusEffect(Target target)
    {
        owner = target;
    }

    abstract public void Active(int count);
}