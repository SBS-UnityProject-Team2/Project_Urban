public static class Ruin
{
    #region Normal
    public static void Ignition(Actor target)
    {
        Attack(target, 6);
        GiveEffect(target, StatusEffectName.Burn, -3, 3);
    }

    public static void MoltenArms(Actor target)
    {
        Attack(target, 15);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Burn, -3, 5);
    }

    public static void Ember(Actor target)
    {
        Attack(target, 6);

        ActionPayload payload = new()
        {
            actionId = ActorAction.CopyCard,
            source = Battle.Instance.Player,
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(Location.Deck);
        payload.Write(CardName.Ember);

        ActionBus.Dispatch(payload);
    }

    public static void Inferno(Actor target)
    {
        int count = Battle.Instance.Deck.ExtinctCardList.Count;
        Attack(target, count * 7);
    }

    public static void Backdraft(Actor target)
    {
        foreach(Monster monster in Battle.Instance.Monsters.List)
        {
            StatusEffect burn = monster.Status.EffectList.GetEffect(StatusEffectName.Burn);
            if (burn == null)
                continue;

            for (int i = 0; i < burn.Stack; i++)
                Attack(monster, 6);            
        }

        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        durPayload.AddTarget(Battle.Instance.Player);
        foreach(Monster monster in Battle.Instance.Monsters.List)
            durPayload.AddTarget(monster);
            
        durPayload.Write(StatusEffectName.Burn);
        durPayload.Write(-3);
        ActionBus.Dispatch(durPayload);

        ActionPayload stPayload = new()
        {
            actionId = ActorAction.GiveBuffSta,
            source = Battle.Instance.Player,
        };
        stPayload.AddTarget(Battle.Instance.Player);
        foreach(Monster monster in Battle.Instance.Monsters.List)
            stPayload.AddTarget(monster);
        stPayload.Write(StatusEffectName.Burn);
        stPayload.Write(5);
        ActionBus.Dispatch(stPayload);
    }

    public static void BlazeBarrier(Actor target)
    {
        AddProtect(Battle.Instance.Player, 10);
        ClearBuff(Battle.Instance.Player, StatusEffectName.KineticVeil);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Refined, 2, 1);
    }

    public static void Reforge(Actor target)
    {
        AddProtect(Battle.Instance.Player, 5);

        StatusEffect burn = Battle.Instance.Player.Status.EffectList.GetEffect(StatusEffectName.Burn);
        if (burn != null)
            AddProtect(Battle.Instance.Player, burn.Stack * 2);

        ClearBuff(Battle.Instance.Player, StatusEffectName.KineticVeil);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Refined, 2, 1);
    }

    public static void Incendiary(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.LoadedIncendiary, 1, 2);
    }

    public static void HeatUp(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.Reinforce, -2, 2);
    }

    public static void Overheat(Actor target)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AddCost,
            source = Battle.Instance.Player,
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(2);

        ActionBus.Dispatch(payload);

        GiveEffect(Battle.Instance.Player, StatusEffectName.Burn, -3, 4);
    }

    public static void Cinder(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.Searing, -2, 1);
    }

    public static void Stigma(Actor target)
    {
        GiveEffect(target, StatusEffectName.Branded, 1, 1);
    }

    public static void OilSplash(Actor target)
    {
        StatusEffect burn =target.Status.EffectList.GetEffect(StatusEffectName.Burn);
        if (burn == null)
            return;

        GiveEffect(target, StatusEffectName.Burn, 0, burn.Stack);
    }
    #endregion

    #region Plus
    public static void IgnitionPlus(Actor target)
    {
        Attack(target, 8);
        GiveEffect(target, StatusEffectName.Burn, -3, 3);
    }

    public static void MoltenArmsPlus(Actor target)
    {
        Attack(target, 18);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Burn, -3, 6);
    }

    public static void EmberPlus(Actor target)
    {
        Attack(target, 8);

        ActionPayload payload = new()
        {
            actionId = ActorAction.CopyCard,
            source = Battle.Instance.Player,
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(Location.Deck);
        payload.Write(CardName.Ember);

        ActionBus.Dispatch(payload);
    }

    public static void InfernoPlus(Actor target)
    {
        int count = Battle.Instance.Deck.ExtinctCardList.Count;
        Attack(target, count * 10);
    }

    public static void BackdraftPlus(Actor target)
    {
        foreach(Monster monster in Battle.Instance.Monsters.List)
        {
            StatusEffect burn = monster.Status.EffectList.GetEffect(StatusEffectName.Burn);
            if (burn == null)
                continue;

            for (int i = 0; i < burn.Stack; i++)
                Attack(monster, 8);            
        }

        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        durPayload.AddTarget(Battle.Instance.Player);
        foreach(Monster monster in Battle.Instance.Monsters.List)
            durPayload.AddTarget(monster);
            
        durPayload.Write(StatusEffectName.Burn);
        durPayload.Write(-3);
        ActionBus.Dispatch(durPayload);

        ActionPayload stPayload = new()
        {
            actionId = ActorAction.GiveBuffSta,
            source = Battle.Instance.Player,
        };
        stPayload.AddTarget(Battle.Instance.Player);
        foreach(Monster monster in Battle.Instance.Monsters.List)
            stPayload.AddTarget(monster);
        stPayload.Write(StatusEffectName.Burn);
        stPayload.Write(5);
        ActionBus.Dispatch(stPayload);
    }

    public static void BlazeBarrierPlus(Actor target)
    {
        AddProtect(Battle.Instance.Player, 13);
        ClearBuff(Battle.Instance.Player, StatusEffectName.KineticVeil);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Refined, 2, 1);
    }

    public static void ReforgePlus(Actor target)
    {
        AddProtect(Battle.Instance.Player, 6);

        StatusEffect burn = Battle.Instance.Player.Status.EffectList.GetEffect(StatusEffectName.Burn);
        if (burn != null)
            AddProtect(Battle.Instance.Player, burn.Stack * 3);

        ClearBuff(Battle.Instance.Player, StatusEffectName.KineticVeil);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Refined, 2, 1);
    }

    public static void IncendiaryPlus(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.LoadedIncendiary, 1, 4);
    }

    public static void HeatUpPlus(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.Reinforce, -2, 3);
    }

    public static void OverheatPlus(Actor target)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AddCost,
            source = Battle.Instance.Player,
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(3);

        ActionBus.Dispatch(payload);

        GiveEffect(Battle.Instance.Player, StatusEffectName.Burn, -3, 4);
    }

    public static void CinderPlus(Actor target)
    {
        GiveEffect(Battle.Instance.Player, StatusEffectName.Searing, -2, 1);
    }

    public static void StigmaPlus(Actor target)
    {
        GiveEffect(target, StatusEffectName.Branded, 1, 2);
    }

    public static void OilSplashPlus(Actor target)
    {
        StatusEffect burn =target.Status.EffectList.GetEffect(StatusEffectName.Burn);
        if (burn == null)
            return;

        GiveEffect(target, StatusEffectName.Burn, 0, burn.Stack * 2);
    }
    #endregion

    #region Helper
    private static void Attack(Actor target, int damage, ElementType elementType = ElementType.Ruin)
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
        stPayload.Write(duration);
        ActionBus.Dispatch(stPayload);
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