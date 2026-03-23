abstract public class ActiveStatusEffect : StatusEffect
{
    protected ActiveStatusEffect(Actor owner) : base(owner)
    {
    }

    abstract public void Active(int count);
}