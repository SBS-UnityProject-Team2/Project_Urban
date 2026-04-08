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

            if (owner.Status.EffectList.GetEffect(StatusEffectName.FrozenResistance).IsActive)
                return;

            ActionPayload payload = new()
            {
                actionId = ActorAction.GiveBuffDur,
                source = owner,
            };
            payload.AddTarget(owner);
            payload.Write(StatusEffectName.Frozen);
            payload.Write(frozenDuration);
            ActionBus.Dispatch(payload);
        }
    }
}