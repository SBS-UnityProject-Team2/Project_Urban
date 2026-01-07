public abstract class StatusEffect
{
    abstract public StatusEffectName Name { get; }
    abstract public void Apply(Target target);
}