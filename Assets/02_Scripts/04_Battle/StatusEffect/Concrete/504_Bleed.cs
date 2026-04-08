using UnityEngine;

public class Bleed : StackEffect, IHealChange
{
    public override StatusEffectName Name => StatusEffectName.Bleed;
    public Bleed(Actor owner) : base(owner)
    {
        owner.EventBus.AddEventListener(ActorEvent.TurnEnd, HandleTurnEnd);
    }

    private void HandleTurnEnd(EventPayload eventPayload)
    {
        if (!isActive) return;

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