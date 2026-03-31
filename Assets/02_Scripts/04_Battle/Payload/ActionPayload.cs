using System.Collections.Generic;

public class AtkDmgPayload : ActionPayload
{
    public int damage;
    public ElementType elementType;

    public AtkDmgPayload() { actionId = ActorAction.AtkDmg; }
}

public class AtkFixedDmgPayload : ActionPayload
{
    public int damage;
    public ElementType elementType;

    public AtkFixedDmgPayload() { actionId = ActorAction.AtkFixedDmg; }
}

public class AtkLossHpPayload : ActionPayload
{
    public int damage;

    public AtkLossHpPayload() { actionId = ActorAction.AtkLossHp; }
}

public class DmgAdjustPayload : ActionPayload
{
    public int maxDamage;

    public DmgAdjustPayload() { actionId = ActorAction.DmgAdjust; }
}

public class DmgRateAdjustPayload : ActionPayload
{
    public float maxDamageRate;

    public DmgRateAdjustPayload() { actionId = ActorAction.DmgRateAdjust; }
}

public class AddBlockPayload : ActionPayload
{
    public int block;

    public AddBlockPayload() { actionId = ActorAction.AddBlock; }
}

public class HealHpPayload : ActionPayload
{
    public int healPoint;

    public HealHpPayload() { actionId = ActorAction.HealHp; }
}

public class AddMaxHpPayload : ActionPayload
{
    public int maxHpPoint;

    public AddMaxHpPayload() { actionId = ActorAction.AddMaxHp; }
}

public class AddCostPayload : ActionPayload
{
    public int costPoint;

    public AddCostPayload() { actionId = ActorAction.AddCost; }
}

public class ChangeElementPayload : ActionPayload
{
    public ElementType elementType;

    public ChangeElementPayload() { actionId = ActorAction.ChangeElement; }
}


public class TakenDmgRateAdjustPayload : ActionPayload
{
    public float damageRate;

    public TakenDmgRateAdjustPayload() { actionId = ActorAction.TakenDmgRateAdjust; }
}

public class ShuffleDeckPayload : ActionPayload
{
    public ShuffleDeckPayload() { actionId = ActorAction.ShuffleDeck; }
}

public class MoveCardFromDeckPayload : ActionPayload
{
    public int cardCount;
    public Location to;
    
    public MoveCardFromDeckPayload() { actionId = ActorAction.MoveCardFromDeck; }
}

public class MoveCardFromHandPayload : ActionPayload
{
    public int cardCount;
    public Location to;

    public MoveCardFromHandPayload() { actionId = ActorAction.MoveCardFromHand; }
}

public class MoveCardFromDiscardPayload : ActionPayload
{
    public int cardCount;
    public Location to;

    public MoveCardFromDiscardPayload() { actionId = ActorAction.MoveCardFromDiscard; }
}

public class MoveCardFromExhaustPayload : ActionPayload
{
    public int cardCount;
    public Location to;

    public MoveCardFromExhaustPayload() { actionId = ActorAction.MoveCardFromExhaust; }
}

public class SearchCardPayload : ActionPayload
{
    public int cardCount;

    public SearchCardPayload() { actionId = ActorAction.SearchCard; }
}

public class MoveSelectedCardPayload : ActionPayload
{
    public MoveSelectedCardPayload() { actionId = ActorAction.MoveSelectedCard; }
}

public class DrawCountPayload : ActionPayload
{
    public int drawCount;

    public DrawCountPayload() { actionId = ActorAction.DrawCount; }
}

public class SetCardCostPayload : ActionPayload
{
    public int cardInstanceId;
    public int costPoint;

    public SetCardCostPayload() { actionId = ActorAction.SetCardCost; }
}

public class AddCardCostPayload : ActionPayload
{
    public int cardInstanceId;
    public int costPoint;

    public AddCardCostPayload() { actionId = ActorAction.AddCardCost; }
}

public class ReduceCardCostPayload : ActionPayload
{
    public int cardInstanceId;
    public int costPoint;

    public ReduceCardCostPayload() { actionId = ActorAction.ReduceCardCost; }
}

public class RandomizeCardCostPayload : ActionPayload
{
    public int cardInstanceId;
    public int maxCostPoint;        

    public RandomizeCardCostPayload() { actionId = ActorAction.RandomizeCardCost; }
}

public class ResetCardCostPayload : ActionPayload
{
    public int cardInstanceId;
    
    public ResetCardCostPayload() { actionId = ActorAction.ResetCardCost; }
}

public class CreateCardPayload : ActionPayload
{
    public CardName cardName;
    public Location to;

    public CreateCardPayload() { actionId = ActorAction.CreateCard; }
}

public class CopyCardPayload : ActionPayload
{
    public CardName cardName;
    public Location to;

    public CopyCardPayload() { actionId = ActorAction.CopyCard; }
}

public class TransformCardPayload : ActionPayload
{
    public CardName before;
    public CardName after;

    public TransformCardPayload() { actionId = ActorAction.TransformCard; }
}

public class ResetCardTransformPayload : ActionPayload
{
    public ResetCardTransformPayload() { actionId = ActorAction.ResetCardTransform; }
}

public class GiveBuffDurPayload : ActionPayload
{
    public StatusEffectName effectName;
    public int duration;

    public GiveBuffDurPayload() { actionId = ActorAction.GiveBuffDur; }
}

public class GiveBuffStaPayload : ActionPayload
{
    public StatusEffectName effectName;
    public int stack;

    public GiveBuffStaPayload() { actionId = ActorAction.GiveBuffSta; }
}

public class RemoveBuffDurPayload : ActionPayload
{
    public StatusEffectName effectName;
    public int duration;

    public RemoveBuffDurPayload() { actionId = ActorAction.RemoveBuffDur; }
}

public class RemoveBuffStaPayload : ActionPayload
{
    public StatusEffectName effectName;
    public int stack;

    public RemoveBuffStaPayload() { actionId = ActorAction.RemoveBuffSta; }
}

public class ClearBuffsPayload : ActionPayload
{
    public StatusEffectName effectName;

    public ClearBuffsPayload() { actionId = ActorAction.ClearBuffs; }
}

public class CancelBuffPayload : ActionPayload
{
    public StatusEffectName before;
    public StatusEffectName after;

    public CancelBuffPayload() { actionId = ActorAction.CancelBuff; }
}

public class ActionSkipPayload : ActionPayload
{
    public ActionSkipPayload() { actionId = ActorAction.ActionSkip; }
}

public class ActionPayload : Payload
{
    public ActorAction actionId;
    public Actor source;
    public List<Actor> targets = new();

    public override void Init()
    {
        base.Init();
        
        source   = null;
        targets.Clear();
    }

    public void AddTarget(Actor actor)
    {
        targets.Add(actor);
    }
}