abstract public class ModifierEffect
{
    protected int stack;
    abstract public void Modify(Status status, int count = 1);
}