abstract public class ActiveStatusEffect : StatusEffect
{
    protected readonly Actor owner;

    public ActiveStatusEffect(Actor target) : base()
    {
        owner = target;
    }

    abstract public void Active(int count);
}