using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

public enum ActorAction
{
    None = 0,
    // 공격 로직
    AtkDmg = 50001,
    AtkFixedDmg,
    AtkLossHp,
    DmgAdjust,
    DmgRateAdjust,
    // 방어 및 지원 로직
    AddBlock = 50101,
    HealHp,
    AddMaxHp,
    AddCost,
    ChangeElement,
    TakenDmgRateAdjust,
    // 카드 조작 관련
    ShuffleDeck = 50200, 
    MoveCard,
    SearchCard,
    MoveSelectedCard,
    MoveTempToTarget,
    ControlTempTarget,
    DrawCount,
    // 코스트 조작 관련
    SetCardCost = 50206,
    AddCardCost,
    RandomizeCardCost,
    ResetCardCost,
    // 카드 생성 관련
    CreateCard = 50210,
    CopyCard,
    TransformCard,
    ResetCardTransform,
    // 버프 디버프 관련
    GiveBuff = 50301,
    RemoveBuff,
    ClearBuffs,
    UpdateBuff,
    CancelBuff,
    ActionSkip,
}

public static class ActionBus
{
    private static readonly float damageModifier = 0.3f;

    private static readonly Dictionary<ActorAction, Action<ActionPayload>> dispatchTable;

    static ActionBus()
    {
        dispatchTable = new();

        MethodInfo [] methods = typeof(ActionBus).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

        foreach (ActorAction action in Enum.GetValues(typeof(ActorAction)))
        {
            if (action == ActorAction.None) continue;

            string methodName = action.ToString();
            MethodInfo method = Array.Find(methods, method => method.Name == methodName);
            Debug.Assert(method != null);

            Delegate handler = Delegate.CreateDelegate(typeof(Action<ActionPayload>), method);
            dispatchTable.Add(action, (Action<ActionPayload>)handler);            
        }
    }

    public static void Dispatch(ActionPayload payload)
    {
        if (dispatchTable.TryGetValue(payload.actionId, out var handler))
            handler(payload);
    }

    private static void AtkDmg(ActionPayload payload)
    {
        int damage = payload.Read<int>();
        Element element = payload.Read<Element>();

        foreach (Actor target in payload.targets)
        {
            damage = ModifyDamageByElement(damage, target.Status.Element, element);
            // 1. 버프 및 디버프 데미지 증가
            // 2. 증강 버프 수치 부여
            

            Health targetHealth = target.Status.Health;
            int remainingDamage = damage;
            if (targetHealth.Block > 0)
            {
                remainingDamage -= targetHealth.Block;
                targetHealth.Block -= damage;

                if (targetHealth.Block == 0)
                {
                    // break 이벤트 발생시키기
                }
            }

            targetHealth.CurHp -= remainingDamage;
            // TakeDamage 이벤트 발생 

            if (targetHealth.CurHp == 0)
            {
                // dead 이벤트 발생
            }            
        }
    }

    private static void AtkFixedDmg(ActionPayload payload)
    {
        int damage = payload.Read<int>();
        Element element = payload.Read<Element>();

        foreach (Actor target in payload.targets)
        {
            damage = ModifyDamageByElement(damage, target.Status.Element, element);
            // 1. 버프 및 디버프 데미지 증가
            // 2. 증강 버프 수치 부여

            Health targetHealth = target.Status.Health;
            targetHealth.CurHp -= damage;
            // TakeDamage 이벤트 발생 

            if (targetHealth.CurHp == 0)
            {
                // dead 이벤트 발생
            }            
        }
    }

    private static void AtkLossHp(ActionPayload payload)
    {
        int damage = payload.Read<int>();
        
        foreach (Actor target in payload.targets)
        {
            Health targetHealth = target.Status.Health;
            targetHealth.CurHp -= damage;
            
            // TakeDamage 이벤트 발생 

            if (targetHealth.CurHp == 0)
            {
                // dead 이벤트 발생
            }  
        }
    }

    private static void AddBlock(ActionPayload payload)
    {
        int block = payload.Read<int>();

        foreach (Actor actor in payload.targets)
            actor.Status.Health.Block += block;
    }

    private static void HealHp(ActionPayload payload)
    {
        int healPoint = payload.Read<int>();
    }

    private static void AddMaxHp(ActionPayload payload)
    {
        int maxHp = payload.Read<int>();
    }

    private static void AddCost(ActionPayload payload)
    {
        int cost = payload.Read<int>();
    }

    private static void ChangeElement(ActionPayload payload)
    {
        Element element = payload.Read<Element>();

        foreach (Actor target in payload.targets)
            target.Status.Element = element;
    }

    private static void ShuffleDeck(ActionPayload payload)
    {
        Battle.Instance.Deck.Shuffle();
    }

    private static void MoveCard(ActionPayload payload)
    {
        int amount = payload.Read<int>();
        Location from = payload.Read<Location>();
        Location to = payload.Read<Location>();

        // 이동 규칙 다시 확인하기
    }

    private static void SearchCard(ActionPayload payload)
    {
        
    }

    private static void MoveSelectedCard(ActionPayload payload)
    {
        
    }

    private static void DmgAdjust(ActionPayload payload)
    {
        
    }

    private static void DmgRateAdjust(ActionPayload payload)
    {
        
    }

    private static void TakenDmgRateAdjust(ActionPayload payload)
    {
        
    }

    private static void MoveTempToTarget(ActionPayload payload)
    {
        
    }

    private static void ControlTempTarget(ActionPayload payload)
    {
        
    }

    private static void DrawCount(ActionPayload payload)
    {
        
    }

    private static void SetCardCost(ActionPayload payload)
    {
        
    }

    private static void AddCardCost(ActionPayload payload)
    {
        
    }

    private static void RandomizeCardCost(ActionPayload payload)
    {
        
    }

    private static void ResetCardCost(ActionPayload payload)
    {
        
    }

    private static void CreateCard(ActionPayload payload)
    {
        
    }

    private static void CopyCard(ActionPayload payload)
    {
        
    }

    private static void TransformCard(ActionPayload payload)
    {
        
    }

    private static void ResetCardTransform(ActionPayload payload)
    {
        
    }

    private static void GiveBuff(ActionPayload payload)
    {
        
    }

    private static void RemoveBuff(ActionPayload payload)
    {
        
    }

    private static void ClearBuffs(ActionPayload payload)
    {
        
    }

    private static void UpdateBuff(ActionPayload payload)
    {
        
    }

    private static void CancelBuff(ActionPayload payload)
    {
        
    }

    private static void ActionSkip(ActionPayload payload)
    {
        
    }

    

    private static int ModifyDamageByElement(int hitPoint, Element blockType, Element attackType)
    {
        if (blockType == Element.None) return hitPoint;

        int modifiedDamage = 0;

        if (attackType == Element.Ruin)
        {
            if (blockType == Element.Bio)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (blockType == Element.Psychic)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        else if (attackType == Element.Psychic)
        {
            if (blockType == Element.Ruin)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (blockType == Element.Bio)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        else if (attackType == Element.Bio)
        {
            if (blockType == Element.Psychic)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (blockType == Element.Ruin)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        return hitPoint + modifiedDamage;
    }
}

