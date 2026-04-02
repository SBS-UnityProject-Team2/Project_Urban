public class ResourceTrade : StackEffect
{
    public override StatusEffectName Name => StatusEffectName.ResourceTrade;

    public ResourceTrade(Actor owner) : base(owner) {}

    public override void GiveStack(int stack = 1)
    {
        if (!isActive)
            owner.EventBus.AddEventListener(ActorEvent.InitDraw, HandleDraw);

        base.GiveStack(stack);
    }

    public override void Clear()
    {
        base.Clear();
        owner.EventBus.RemoveEventListener(ActorEvent.InitDraw, HandleDraw);
    }

    public void HandleDraw(EventPayload eventPayload)
    {   
        ActionPayload movePayload = new()
        {
            actionId = ActorAction.MoveCardFromHand,
            source = owner,
        };
        movePayload.Write(Location.DiscardPile);
        movePayload.Write(stack);
        movePayload.AddTarget(owner);
        ActionBus.Dispatch(movePayload);

        // 카드 버린것을 확인하고 실행해야됨
        ActionPayload healHpPayload = new()
        {
            actionId = ActorAction.HealHp,
            source = owner,
        };
        healHpPayload.Write(stack);
        healHpPayload.AddTarget(owner);
        ActionBus.Dispatch(healHpPayload);

        ActionPayload addCostPayload = new()
        {
            actionId = ActorAction.AddCost,
            source = owner,
        };
        addCostPayload.Write(stack);
        addCostPayload.AddTarget(owner);
        ActionBus.Dispatch(addCostPayload);
    }
}