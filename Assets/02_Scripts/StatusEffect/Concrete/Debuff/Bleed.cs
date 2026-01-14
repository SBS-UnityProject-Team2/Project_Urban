public class Bleed : StackStatusEffect
{
    private readonly Target owner;

    public override StatusEffectName Name => StatusEffectName.Bleed;
    public override int StatusNumber => stack;

    public Bleed(Target target)
    {
        owner = target;

        target.OnTurnEnd.AddListener(HandleTurnEnd);
    }

    private void HandleTurnEnd()
    {
        if (!IsActive) return;

        owner.DebuffDamage(stack);
    }
}