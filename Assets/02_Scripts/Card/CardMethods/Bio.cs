using System;
using Cysharp.Threading.Tasks;

public static class Bio
{
    #region Normal
    public static async UniTask DoubleEdge(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 16);
        Attack(Battle.Instance.Player, 8);
    }

    public static async UniTask Plague(Actor target, Func<Actor, UniTask> effectPlay)
    {
    }

    public static async UniTask ThornWhip(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask AbsorbingStrike(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask DistortedSlay(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask SpikyBush(Actor target, Func<Actor, UniTask> effectPlay)
    {
    }

    public static async UniTask ElasticWall(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask Blooming(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask SurgingLife(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask CellChange(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask Cycle(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask EnfeebleSludge(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }
    #endregion

    #region Plus
    public static async UniTask DoubleEdgePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask PlaguePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask ThornWhipPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask AbsorbingStrikePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask DistortedSlayPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask SpikyBushPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask ElasticWallPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask BloomingPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask SurgingLifePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask CellChangePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask CyclePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
     
    }

    public static async UniTask EnfeebleSludgePlus(Actor target, Func<Actor, UniTask> effectPlay)
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