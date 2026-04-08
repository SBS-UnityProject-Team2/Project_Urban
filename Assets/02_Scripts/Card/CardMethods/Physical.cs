using System;
using Cysharp.Threading.Tasks;

public static class Physical
{
    #region Normal
    public static async UniTask Punch(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 5);
        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Guard(Actor target, Func<Actor, UniTask> effectPlay)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AddBlock,
            source = Battle.Instance.Player
        };
        payload.AddTarget(target);
        payload.Write(5);

        ActionBus.Dispatch(payload);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Shooting(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 7, Battle.Instance.Player.LastUsedElementType);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Strike(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 12);
        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask VileAttack(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 9);
        GiveEffect(target ,StatusEffectName.Weaken, 2, 3);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Assault(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 10);
        GiveEffect(target, StatusEffectName.Frozen, 1, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Rollout(Actor target, Func<Actor, UniTask> effectPlay)
    {
        AddProtect(Battle.Instance.Player, 7);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Preparation, 1, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Maintenance(Actor target, Func<Actor, UniTask> effectPlay)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.DrawCard,
            source = target
        };
        payload.AddTarget(target);
        payload.Write(2);

        ActionBus.Dispatch(payload);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Dummy(Actor target, Func<Actor, UniTask> effectPlay)
    {
        GiveEffect(target, StatusEffectName.Blur, -2, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }
    #endregion

    #region Plus
    public static async UniTask PunchPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 8);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask GuardPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AddBlock,
            source = Battle.Instance.Player
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(8);

        ActionBus.Dispatch(payload);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask ShootingPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 10, Battle.Instance.Player.LastUsedElementType);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask StrikePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 18);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask VileAttackPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 12);
        GiveEffect(target ,StatusEffectName.Weaken, 2, 3);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask AssaultPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 14);
        GiveEffect(target, StatusEffectName.Frozen, 1, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask RolloutPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        Attack(target, 10);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Preparation, 1, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask MaintenancePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.DrawCard,
            source = target
        };
        payload.AddTarget(target);
        payload.Write(2);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask DummyPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        GiveEffect(target, StatusEffectName.Blur, -2, 3);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
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
    #endregion
}