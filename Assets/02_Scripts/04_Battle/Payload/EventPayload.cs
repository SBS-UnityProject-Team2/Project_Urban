public class TurnStartPayload : EventPayload
{
    public TurnStartPayload() { eventId = ActorEvent.TurnStart; }
}

public class TurnEndPayload : EventPayload
{
    public TurnEndPayload() { eventId = ActorEvent.TurnEnd; }
}

public class InitDrawPayload : EventPayload
{
    public int drawCount;

    public InitDrawPayload() { eventId = ActorEvent.InitDraw; }
}

public class DrawPayload : EventPayload
{
    public int drawCount;

    public DrawPayload() { eventId = ActorEvent.Draw; }
}

public class ActionEventPayload : EventPayload
{
    public CardName cardName;

    public ActionEventPayload() { eventId = ActorEvent.Action; }
}

public class AttackEventPayload : EventPayload
{
    public AttackEventPayload() { eventId = ActorEvent.Attack; }
}

public class ProtectPayload : EventPayload
{
    public int blockPoint;

    public ProtectPayload() { eventId = ActorEvent.Protect; }
}

public class GiveEffectPayload : EventPayload
{
    public StatusEffectName effectName;
    public int duration;
    public int stack;

    public GiveEffectPayload() { eventId = ActorEvent.GiveEffect; }
}

public class RemoveEffectPayload : EventPayload
{
    public StatusEffectName effectName;
    public int duration;
    public int stack;

    public RemoveEffectPayload() { eventId = ActorEvent.RemoveEffect; }
}

public class ClearEffectPayload : EventPayload
{
    public StatusEffectName effectName;

    public ClearEffectPayload() { eventId = ActorEvent.ClearEffect; }
}

public class UpdateCurHpPayload : EventPayload
{
    public int updateDelta;

    public UpdateCurHpPayload() { eventId = ActorEvent.UpdateCurHp; }
}

public class UpdateMaxHpPayload : EventPayload
{
    public int updateDelta;

    public UpdateMaxHpPayload() { eventId = ActorEvent.UpdateMaxHp; }
}

public class UpdateCurCostPayload : EventPayload
{
    public int updateDelta;

    public UpdateCurCostPayload() { eventId = ActorEvent.UpdateCurCost; }
}

public class UpdateMaxCostPayload : EventPayload
{
    public int updateDelta;

    public UpdateMaxCostPayload() { eventId = ActorEvent.UpdateMaxCost; }
}

public class UpdateElementPayload : EventPayload
{
    public ElementType beforeType;
    public ElementType afterType;

    public UpdateElementPayload() { eventId = ActorEvent.UpdateElement; }
}

public class DamageTakenPayload : EventPayload
{
    public int damage;

    public DamageTakenPayload() { eventId = ActorEvent.DamageTaken; }
}

public class DamageIncomingPayload : EventPayload
{
    public DamageIncomingPayload() { eventId = ActorEvent.DamageIncoming; }
}

public class AttackDamageTakenPayload : EventPayload
{
    public int damage;

    public AttackDamageTakenPayload() { eventId = ActorEvent.AttackDamageTaken; }
}

public class EffectDamageTakenPayload : EventPayload
{
    public int damage;
    public StatusEffectName effectName;

    public EffectDamageTakenPayload() { eventId = ActorEvent.EffectDamageTaken; }
}

public class BreakPayload : EventPayload
{
    public BreakPayload() { eventId = ActorEvent.Break; }
}

public class DeadPayload : EventPayload
{
    public DeadPayload() { eventId = ActorEvent.Dead; }
}

public class EventPayload : Payload
{
    public ActorEvent eventId;
    public Actor source;
    public Actor target;

    public override void Init()
    {
        base.Init();

        source = null;
        target = null;
    }
}