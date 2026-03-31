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
        MoveCardFromHandPayload movePayload = new()
        {
            source = owner,
            cardCount = stack,
            to = Location.DiscardPile,
        };
        movePayload.AddTarget(owner);
        ActionBus.Dispatch(movePayload);

        // 카드 버린것을 확인하고 실행해야됨
        HealHpPayload healHpPayload = new()
        {
            source = owner,
            healPoint = stack
        };
        healHpPayload.AddTarget(owner);
        ActionBus.Dispatch(healHpPayload);

        AddCostPayload addCostPayload = new()
        {
            source = owner,
            costPoint = stack,
        };
        healHpPayload.AddTarget(owner);
        ActionBus.Dispatch(addCostPayload);
    }
}