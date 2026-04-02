using UnityEngine;

public class Bleed : StackEffect, IHealChange
{
    public override StatusEffectName Name => StatusEffectName.Bleed;
    public Bleed(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive) 
            owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkLossHp,
            source = owner,
        };
        payload.Write(stack);
        payload.AddTarget(owner);
        ActionBus.Dispatch(payload);

        RequestClear();
    }

    public int GetHealDelta(int healPoint)
    {
        int absorbedHeal = Mathf.Min(stack, healPoint);
        stack -= absorbedHeal;

        return -absorbedHeal;
    }
}