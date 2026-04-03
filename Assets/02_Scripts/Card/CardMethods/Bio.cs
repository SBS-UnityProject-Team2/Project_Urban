public static class Bio
{
    #region Normal
    public static void DoubleEdge(Actor target)
    {
        Attack(target, 16);
        Attack(Battle.Instance.Player, 8);
    }

    public static void Plague(Actor target)
    {
    }

    public static void ThornWhip(Actor target)
    {
    }

    public static void AbsorbingStrike(Actor target)
    {
    }

    public static void DistortedSlay(Actor target)
    {
    }

    public static void SpikyBush(Actor target)
    {
    }

    public static void ElasticWall(Actor target)
    {
    }

    public static void Blooming(Actor target)
    {
    }

    public static void SurgingLife(Actor target)
    {
    }

    public static void CellChange(Actor target)
    {
    }

    public static void Cycle(Actor target)
    {
    }

    public static void EnfeebleSludge(Actor target)
    {
    }
    #endregion

    #region Plus
    public static void DoubleEdgePlus(Actor target)
    {
    }

    public static void PlaguePlus(Actor target)
    {
    }

    public static void ThornWhipPlus(Actor target)
    {
    }

    public static void AbsorbingStrikePlus(Actor target)
    {
    }

    public static void DistortedSlayPlus(Actor target)
    {
    }

    public static void SpikyBushPlus(Actor target)
    {
    }

    public static void ElasticWallPlus(Actor target)
    {
    }

    public static void BloomingPlus(Actor target)
    {
    }

    public static void SurgingLifePlus(Actor target)
    {
    }

    public static void CellChangePlus(Actor target)
    {
    }

    public static void CyclePlus(Actor target)
    {
    }

    public static void EnfeebleSludgePlus(Actor target)
    {
    }
    #endregion

    #region Helper
    private static void Attack(Actor target, int damage, ElementType elementType = ElementType.Bio)
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
