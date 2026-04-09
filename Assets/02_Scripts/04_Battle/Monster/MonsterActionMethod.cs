using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public static class MonsterActionMethod
{
    private static readonly Dictionary<int, Func<Actor, UniTask>> actionMap = new()
    {
        // Attack (80xx)
        { 8000, Charge },
        { 8001, Bite },
        { 8002, Screw },
        { 8003, ScarpGun },
        { 8004, CrudeMolotv },
        { 8005, BurntArm },
        { 8006, PsychicBeat },
        { 8007, RepulsiveMatrix },
        { 8008, Bash },
        { 8009, CorrosiveShock },
        { 8010, MultipleFire },

        // Defense (82xx)
        { 8200, Curl },
        { 8201, Smoke },
        { 8202, ScrapShield },
        { 8204, RegenCell },
        { 8205, Barrier },
        { 8206, Rush },

        // Buff (84xx)
        { 8400, Sharpening },
        { 8401, EnhanceBody },
        { 8402, GhostPill },
        { 8403, ToxicSpore },
        { 8404, Enrage },
        { 8405, Reload },
        { 8406,DefiledInjection },

        // Debuff (86xx)
        { 8600, Roar },
        { 8601, ArmorBreak },
    };

    public static async UniTask Execute(int actionId, Actor source)
    {
        if (actionMap.TryGetValue(actionId, out Func<Actor, UniTask> action))
            await action(source);
    }

    #region Attack (80xx)
    public static async UniTask Charge(Actor source) // 8000
    {
        Attack(source, ElementType.None, 6);

        await EffectManager.Instance.PlayerHitEffect();
    }

    public static async UniTask Bite(Actor source) // 8001
    {
        Attack(source, ElementType.None, 3);
        GiveEffect(source, Battle.Instance.Player, StatusEffectName.Bleed, 1, 3);

        await EffectManager.Instance.PlayerHitEffect();
    }

    public static async UniTask Screw(Actor source) // 8002
    {
        for (int i = 0; i < 2; i++)
        {
            Attack(source, ElementType.None, 5);

            await EffectManager.Instance.PlayerHitEffect();
        }
    }

    public static async UniTask ScarpGun(Actor source) // 8003
    {
        Attack(source, ElementType.None, 12);

        await EffectManager.Instance.PlayerHitEffect();
    }

    public static async UniTask CrudeMolotv(Actor source) // 8004
    {
        for (int i = 0; i < 2; i++)
        {
            Attack(source, ElementType.Ruin, 4);
            await EffectManager.Instance.PlayerHitEffect();
        }

        GiveEffect(source, Battle.Instance.Player, StatusEffectName.Burn, -3, 2);
    }

    public static async UniTask BurntArm(Actor source) // 8005
    {
        Attack(source, ElementType.Ruin, 10);
        GiveEffect(source, source, StatusEffectName.Reinforce, -2, 3);

        await EffectManager.Instance.PlayerHitEffect();
    }

    public static async UniTask PsychicBeat(Actor source) // 8006
    {
        for (int i = 0; i < 2; i++)
        {
            Attack(source, ElementType.Psychic, 5);
            await EffectManager.Instance.PlayerHitEffect();
        }
    }

    public static async UniTask RepulsiveMatrix(Actor source) // 8007
    {
        Attack(source, ElementType.Psychic, 9);
        AddBlock(source, 9);

        await EffectManager.Instance.PlayerHitEffect();
    }

    public static async UniTask Bash(Actor source) // 8008
    {
        Attack(source, ElementType.Bio, 9);
        GiveEffect(source, Battle.Instance.Player, StatusEffectName.Weaken, 2, 3);

        await EffectManager.Instance.PlayerHitEffect();
    }

    public static async UniTask CorrosiveShock(Actor source) // 8009
    {
        for (int i = 0; i < 2; i++)
        {
            Attack(source, ElementType.Bio, 7);
            await EffectManager.Instance.PlayerHitEffect();
        }
        
        GiveEffect(source, Battle.Instance.Player, StatusEffectName.Broken, 2, 3);
    }

    public static async UniTask MultipleFire(Actor source) // 8010
    {
        for (int i = 0; i < 5; i++)
        {
            Attack(source, ElementType.None, 3);
            await EffectManager.Instance.PlayerHitEffect();
        }
    }
    #endregion

    #region Defense (82xx)
    public static async UniTask Curl(Actor source) // 8200
    {
        AddBlock(source, 7);

        await UniTask.CompletedTask;
    }

    public static async UniTask Smoke(Actor source) // 8201
    {
        AddBlock(source, 10);

        await UniTask.CompletedTask;
    }

    public static async UniTask ScrapShield(Actor source) // 8202
    {
        AddBlock(source, 15);
        GiveEffect(source, source, StatusEffectName.Broken, 2, 3);

        await UniTask.CompletedTask;
    }

    public static async UniTask RegenCell(Actor source) // 8204
    {
        AddBlock(source , 7);
        GiveEffect(source, source, StatusEffectName.Regeneration, -3, 2);

        await UniTask.CompletedTask;
    }

    public static async UniTask Barrier(Actor source) // 8205
    {
        AddBlock(source, 12);

        await UniTask.CompletedTask;
    }

    public static async UniTask Rush(Actor source) // 8206
    {
        AddBlock(source, 8);
        GiveEffect(source, source, StatusEffectName.Reinforce, -2, 2);

        await UniTask.CompletedTask;
    }
    #endregion

    #region Buff (84xx)
    public static async UniTask Sharpening(Actor source) // 8400
    {
        GiveEffect(source, source, StatusEffectName.Burst, -2, 9);

        await UniTask.CompletedTask;
    }

    public static async UniTask EnhanceBody(Actor source) // 8401
    {
        GiveEffect(source, source, StatusEffectName.Armor, -2, 2);

        await UniTask.CompletedTask;
    }

    public static async UniTask GhostPill(Actor source) // 8402
    {
        GiveEffect(source, source, StatusEffectName.Blur, -2, 1);

        await UniTask.CompletedTask;
    }

    public static async UniTask ToxicSpore(Actor source) // 8403
    {
        await UniTask.CompletedTask;
    }

    public static async UniTask Enrage(Actor source) // 8404
    {
        GiveEffect(source, source, StatusEffectName.Reinforce, -2, 3);

        await UniTask.CompletedTask;
    }

    public static async UniTask Reload(Actor source) // 8405
    {
        GiveEffect(source, source, StatusEffectName.LoadedIncendiary, 1, 4);

        await UniTask.CompletedTask;
    }

    public static async UniTask DefiledInjection(Actor source) // 8406
    {
        GiveEffect(source, source, StatusEffectName.Reinforce, -2, 4);

        ActionPayload payload = new()
        {
            actionId = ActorAction.ChangeElement,
            source = source,
        };
        payload.AddTarget(source);
        payload.Write(ElementType.Bio);

        ActionBus.Dispatch(payload);

        await UniTask.CompletedTask;
    }
    #endregion

    #region Debuff (86xx)
    public static async UniTask Roar(Actor source) // 8600
    {
        GiveEffect(source, Battle.Instance.Player, StatusEffectName.Weaken, 2, 3);

        await UniTask.CompletedTask;
    }

    public static async UniTask ArmorBreak(Actor source) // 8601
    {
        GiveEffect(source, Battle.Instance.Player, StatusEffectName.Broken, 3, 3);

        await UniTask.CompletedTask;
    }
    #endregion

    #region Helper
    private static void Attack(Actor source, ElementType elementType, int damage)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AtkDmg,
            source = source
        };
        payload.AddTarget(Battle.Instance.Player);
        payload.Write(elementType);
        payload.Write(damage);

        ActionBus.Dispatch(payload);
    }

    private static void AddBlock(Actor source, int blockPoint)
    {
        ActionPayload payload = new()
        {
            actionId = ActorAction.AddBlock,
            source = source
        };
        payload.AddTarget(source);
        payload.Write(blockPoint);

        ActionBus.Dispatch(payload);
    }

    private static void GiveEffect(Actor source, Actor target, StatusEffectName effectName, int duration, int stack)
    {
        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = source
        };
        durPayload.AddTarget(target);
        durPayload.Write(effectName);
        durPayload.Write(duration);
        ActionBus.Dispatch(durPayload);

        ActionPayload stPayload = new()
        {
            actionId = ActorAction.GiveBuffSta,
            source = source
        };
        stPayload.AddTarget(target);
        stPayload.Write(effectName);
        stPayload.Write(stack);

        ActionBus.Dispatch(stPayload);
    }
    #endregion
}
