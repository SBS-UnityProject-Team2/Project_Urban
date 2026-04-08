using System;

public static class Psychic
{
    #region Normal
    public static void GlacierWedge(Actor target)
    {
        Attack(target, 6);
        GiveEffect(target , StatusEffectName.Frost, -2, 1);
    }

    public static void FlowArrow(Actor target)
    {
        Attack(target, 8);

        StatusEffect frost = target.Status.EffectList.GetEffect(StatusEffectName.Frost);
        StatusEffect frozen = target.Status.EffectList.GetEffect(StatusEffectName.Frozen);

        if (frost != null || frozen != null)
        {
            ActionPayload payload = new()
            {
                actionId = ActorAction.DrawCard,
                source = Battle.Instance.Player
            };
            payload.Write(1);

            ActionBus.Dispatch(payload);
        }
    }

    public static void EnergyNeedle(Actor target)
    {
        Attack(target, 7);

        ActionPayload payload = new()
        {
            actionId = ActorAction.AddCost,
            source = Battle.Instance.Player,
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(Battle.Instance.Player.Status.Cost.CurCost == 0 ? 2 : 1);

        ActionBus.Dispatch(payload);
    }

    public static void Pulse(Actor target)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkDmg,
            source = Battle.Instance.Player
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            payload.AddTarget(monster);
        payload.Write(ElementType.Psychic);
        payload.Write(8);
        ActionBus.Dispatch(payload);


        ActionPayload payload1 = new()
        {
            actionId = ActorAction.AtkDmg,
            source = Battle.Instance.Player
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
        {
            if (monster.Status.EffectList.GetEffect(StatusEffectName.Frozen) == null)
                continue;

            payload1.AddTarget(monster);
        }   
        
        payload1.Write(ElementType.Psychic);
        payload1.Write(8);

        if (payload1.targets.Count > 0)
            ActionBus.Dispatch(payload1);
    }

    public static void KineticGrasp(Actor target)
    {
        Attack(target, 24);
    }

    public static void IceShield(Actor target)
    {
        AddProtect(Battle.Instance.Player, 10);

        ClearBuff(Battle.Instance.Player, StatusEffectName.Refined);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.KineticVeil, 2, 1);
    }

    public static void ElectricField(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.ElectricVeil, -2, 2);
    }

    public static void AccelConcoction(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.Acceleration, 3, 1);
    }

    public static void SuperConducter(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.Nullification, 1, 1);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Frozen, 1, 1);
    }

    public static void Anxiolytic(Actor target)
    {
    }

    public static void CryoPowder(Actor target)
    {
        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            durPayload.AddTarget(monster);
        durPayload.Write(StatusEffectName.Frozen);
        durPayload.Write(1);
        ActionBus.Dispatch(durPayload);

        ActionPayload stPayload = new()
        {
            actionId = ActorAction.GiveBuffSta,
            source = Battle.Instance.Player,
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            stPayload.AddTarget(monster);
        stPayload.Write(StatusEffectName.Frozen);
        stPayload.Write(1);
        ActionBus.Dispatch(stPayload);
    }

    public static void Disturb(Actor target)
    {
        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            durPayload.AddTarget(monster);
        durPayload.Write(StatusEffectName.Broken);
        durPayload.Write(2);
        ActionBus.Dispatch(durPayload);

        ActionPayload stPayload = new()
        {
            actionId = ActorAction.GiveBuffSta,
            source = Battle.Instance.Player,
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            stPayload.AddTarget(monster);
        stPayload.Write(StatusEffectName.Broken);
        stPayload.Write(3);
        ActionBus.Dispatch(stPayload);
    }
    #endregion

    #region Plus
    public static void GlacierWedgePlus(Actor target)
    {
        Attack(target, 8);
        GiveEffect(target , StatusEffectName.Frost, -2, 1);
    }

    public static void FlowArrowPlus(Actor target)
    {
        Attack(target, 10);

        StatusEffect frost = target.Status.EffectList.GetEffect(StatusEffectName.Frost);
        StatusEffect frozen = target.Status.EffectList.GetEffect(StatusEffectName.Frozen);

        if (frost != null || frozen != null)
        {
            ActionPayload payload = new()
            {
                actionId = ActorAction.DrawCard,
                source = Battle.Instance.Player
            };
            payload.Write(1);

            ActionBus.Dispatch(payload);
        }
    }

    public static void EnergyNeedlePlus(Actor target)
    {
        Attack(target, 10);

        ActionPayload payload = new()
        {
            actionId = ActorAction.AddCost,
            source = Battle.Instance.Player,
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(Battle.Instance.Player.Status.Cost.CurCost == 0 ? 2 : 1);

        ActionBus.Dispatch(payload);
    }

    public static void PulsePlus(Actor target)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkDmg,
            source = Battle.Instance.Player
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            payload.AddTarget(monster);
        payload.Write(ElementType.Psychic);
        payload.Write(10);
        ActionBus.Dispatch(payload);


        ActionPayload payload1 = new()
        {
            actionId = ActorAction.AtkDmg,
            source = Battle.Instance.Player
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
        {
            if (monster.Status.EffectList.GetEffect(StatusEffectName.Frozen) == null)
                continue;

            payload1.AddTarget(monster);
        }   
        
        payload1.Write(ElementType.Psychic);
        payload1.Write(10);

        if (payload1.targets.Count > 0)
            ActionBus.Dispatch(payload1);
    }

    public static void KineticGraspPlus(Actor target)
    {
        Attack(target, 24);
    }

    public static void IceShieldPlus(Actor target)
    {
        AddProtect(Battle.Instance.Player, 13);

        ClearBuff(Battle.Instance.Player, StatusEffectName.Refined);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.KineticVeil, 2, 1);
    }

    public static void ElectricFieldPlus(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.ElectricVeil, -2, 4);
    }

    public static void AccelConcoctionPlus(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.Acceleration, 3, 1);
    }

    public static void SuperConducterPlus(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.Nullification, 2, 1);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Frozen, 1, 1);
    }

    public static void AnxiolyticPlus(Actor target)
    {
    }

    public static void CryoPowderPlus(Actor target)
    {
        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            durPayload.AddTarget(monster);
        durPayload.Write(StatusEffectName.Frozen);
        durPayload.Write(1);
        ActionBus.Dispatch(durPayload);

        ActionPayload stPayload = new()
        {
            actionId = ActorAction.GiveBuffSta,
            source = Battle.Instance.Player,
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            stPayload.AddTarget(monster);
        stPayload.Write(StatusEffectName.Frozen);
        stPayload.Write(1);
        ActionBus.Dispatch(stPayload);
    }

    public static void DisturbPlus(Actor target)
    {
        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            durPayload.AddTarget(monster);
        durPayload.Write(StatusEffectName.Broken);
        durPayload.Write(3);
        ActionBus.Dispatch(durPayload);

        ActionPayload stPayload = new()
        {
            actionId = ActorAction.GiveBuffSta,
            source = Battle.Instance.Player,
        };
        foreach (Monster monster in Battle.Instance.Monsters.List)
            stPayload.AddTarget(monster);
        stPayload.Write(StatusEffectName.Broken);
        stPayload.Write(3);
        ActionBus.Dispatch(stPayload);
    }
    #endregion

    #region Helper
    private static void Attack(Actor target, int damage, ElementType elementType = ElementType.Psychic)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkDmg,
            source = Battle.Instance.Player,
        };
        payload.AddTarget(target);
        payload.Write(elementType);
        payload.Write(damage);

        ActionBus.Dispatch(payload);
    }

    private static void GiveEffect(Actor target, StatusEffectName effectName, int duration, int stack)
    {
        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        durPayload.AddTarget(target);
        durPayload.Write(effectName);
        durPayload.Write(duration);
        ActionBus.Dispatch(durPayload);

        ActionPayload stPayload = new()
        {
            actionId = ActorAction.GiveBuffSta,
            source = Battle.Instance.Player,
        };
        stPayload.AddTarget(target);
        stPayload.Write(effectName);
        stPayload.Write(stack);
        ActionBus.Dispatch(stPayload);
    }

    private static void AddProtect(Actor target, int blockPoint)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AddBlock,
            source = target
        };
        payload.AddTarget(target);
        payload.Write(blockPoint);

        ActionBus.Dispatch(payload);
    }

    private static void ClearBuff(Actor target, StatusEffectName effectName)
    {
         ActionPayload payload = new()
        {
            actionId = ActorAction.ClearBuffs,
            source = target
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(effectName);
        
        ActionBus.Dispatch(payload);
    }

    #endregion
}
