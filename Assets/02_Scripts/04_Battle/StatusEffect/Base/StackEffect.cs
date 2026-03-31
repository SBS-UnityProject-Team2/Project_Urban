abstract public class StackEffect : StatusEffect
{
    override public int StatusNumber => stack;

    public StackEffect(Actor owner) : base(owner) {}
}