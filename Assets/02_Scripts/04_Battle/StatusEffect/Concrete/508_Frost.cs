public class Frost : StackEffect
{
    private readonly int targetValue = 3;
    private readonly int frozenDuration = 1;
    public override StatusEffectName Name => StatusEffectName.Frost;

    public Frost(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        base.GiveStack(stack);

        if (this.stack == targetValue)
        {
            RemoveStack(targetValue);

            GiveBuffDurPayload payload = new()
            {
                source = owner,
                effectName = StatusEffectName.Frozen,
                duration = frozenDuration  
            };
            payload.AddTarget(owner);
            ActionBus.Dispatch(payload);
        }
    }
}