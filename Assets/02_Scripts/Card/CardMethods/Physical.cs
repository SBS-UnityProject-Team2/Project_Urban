public static class Physical
{
    #region Normal
    public static void Punch(Actor target)
    {
        Attack(target, 5);
    }

    public static void Guard(Actor target)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AddBlock,
            source = Battle.Instance.Player
        };
        payload.AddTarget(target);
        payload.Write(5);

        ActionBus.Dispatch(payload);
    }

    public static void Shooting(Actor target)
    {
        Attack(target, 7, Battle.Instance.Player.LastUsedElementType);
    }

    public static void Strike(Actor target)
    {
        Attack(target, 12);
    }

    public static void VileAttack(Actor target)
    {
        Attack(target, 9);
        GiveEffect(target ,StatusEffectName.Weaken, 2, 3);
    }

    public static void Assault(Actor target)
    {
        Attack(target, 10);
        GiveEffect(target, StatusEffectName.Frozen, 1, 1);
    }

    public static void Rollout(Actor target)
    {
        AddProtect(Battle.Instance.Player, 7);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Preparation, 1, 1);
    }

    public static void Maintenance(Actor target)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.DrawCard,
            source = target
        };
        payload.AddTarget(target);
        payload.Write(2);

        ActionBus.Dispatch(payload);
    }

    public static void Dummy(Actor target)
    {
        GiveEffect(target, StatusEffectName.Blur, -2, 1);
    }
    #endregion

    #region Plus
    public static void PunchPlus(Actor target)
    {
        Attack(target, 8);
    }

    public static void GuardPlus(Actor target)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AddBlock,
            source = Battle.Instance.Player
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(8);

        ActionBus.Dispatch(payload);
    }

    public static void ShootingPlus(Actor target)
    {
        Attack(target, 10, Battle.Instance.Player.LastUsedElementType);
    }

    public static void StrikePlus(Actor target)
    {
        Attack(target, 18);
    }

    public static void VileAttackPlus(Actor target)
    {
        Attack(target, 12);
        GiveEffect(target ,StatusEffectName.Weaken, 2, 3);
    }

    public static void AssaultPlus(Actor target)
    {
        Attack(target, 14);
        GiveEffect(target, StatusEffectName.Frozen, 1, 1);
    }

    public static void RolloutPlus(Actor target)
    {
        Attack(target, 10);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Preparation, 1, 1);
    }

    public static void MaintenancePlus(Actor target)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.DrawCard,
            source = target
        };
        payload.AddTarget(target);
        payload.Write(2);
    }

    public static void DummyPlus(Actor target)
    {
        GiveEffect(target, StatusEffectName.Blur, -2, 2);
    }
    #endregion

    #region Helper
    private static void Attack(Actor target, int damage, ElementType elementType = ElementType.None)
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
        stPayload.Write(duration);
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
    #endregion
}