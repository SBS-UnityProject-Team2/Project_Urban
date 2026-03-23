using System;
using System.Collections.Generic;

public enum ActorAction
{
    None = 0,
    AtkDmg = 50001,
    AtkFixedDmg,
    AtkLossHp,
    AddBlock = 50101,
    HealHp,
    AddMaxHp,
    AddCost
}

public static class ActionBus
{
    private static readonly float damageModifier = 0.3f;

    private static readonly Dictionary<ActorAction, Action<ActionPayload>> dispatchTable;

    static ActionBus()
    {
        dispatchTable = new Dictionary<ActorAction, Action<ActionPayload>>
        {
            { ActorAction.AtkDmg,      AtkDmg      },
            { ActorAction.AtkFixedDmg, AtkFixedDmg },
            { ActorAction.AtkLossHp,   AtkLossHp   },
            { ActorAction.AddBlock,    AddBlock     },
            { ActorAction.HealHp,      HealHp       },
            { ActorAction.AddMaxHp,    AddMaxHp     },
            { ActorAction.AddCost,     AddCost      },
        };
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
        
        // 데미지 처리 후 이벤트 처리도
        // payload.source.EventBus.Dispatch()

        // 데미지 처리
    }

    private static void AtkFixedDmg(ActionPayload payload)
    {
        int damage       = payload.Read<int>();
        Element element  = payload.Read<Element>();
    }

    private static void AtkLossHp(ActionPayload payload)
    {
        int damage = payload.Read<int>();
    }

    private static void AddBlock(ActionPayload payload)
    {
        int block = payload.Read<int>();
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

    private static int ModifyDamageByElement(int hitPoint, Element attackType)
    {
        // Element element = status.Element;

        // if (element == Element.None) return hitPoint;

        // int modifiedDamage = 0;

        // if (attackType == Element.Ruin)
        // {
        //     if (element == Element.Bio)
        //         modifiedDamage = (int)(hitPoint * damageModifier);

        //     if (element == Element.Psychic)
        //         modifiedDamage = (int)(-hitPoint * damageModifier);
        // }

        // else if (attackType == Element.Psychic)
        // {
        //     if (element == Element.Ruin)
        //         modifiedDamage = (int)(hitPoint * damageModifier);

        //     if (element == Element.Bio)
        //         modifiedDamage = (int)(-hitPoint * damageModifier);
        // }

        // else if (attackType == Element.Bio)
        // {
        //     if (element == Element.Psychic)
        //         modifiedDamage = (int)(hitPoint * damageModifier);

        //     if (element == Element.Ruin)
        //         modifiedDamage = (int)(-hitPoint * damageModifier);
        // }

        // return hitPoint + modifiedDamage;

        return 0;
    }
}

