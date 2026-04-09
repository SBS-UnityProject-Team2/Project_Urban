using System;
using Cysharp.Threading.Tasks;

public static class Ruin
{
    #region Normal
    public static async UniTask Ignition(Actor target, Func<Actor, UniTask> effectPlay)
    {

        Attack(target, 6);
        GiveEffect(target, StatusEffectName.Burn, -3, 3);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask MoltenArms(Actor target, Func<Actor, UniTask> effectPlay)
    {

        Attack(target, 15);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Burn, -3, 5);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Ember(Actor target, Func<Actor, UniTask> effectPlay)
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

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Inferno(Actor target, Func<Actor, UniTask> effectPlay)
    {

        int count = Battle.Instance.Deck.ExtinctCardList.Count;
        Attack(target, count * 7);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Backdraft(Actor target, Func<Actor, UniTask> effectPlay)
    {
        foreach (Monster monster in Battle.Instance.Monsters.List)
        {
            StatusEffect burn = monster.Status.EffectList.GetEffect(StatusEffectName.Burn);
            if (burn == null)
                continue;

            for (int i = 0; i < burn.Stack; i++)
            {

                Attack(monster, 6);
            }
        }

        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        durPayload.AddTarget(Battle.Instance.Player);
        foreach (Monster monster in Battle.Instance.Monsters.List)
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
        foreach (Monster monster in Battle.Instance.Monsters.List)
            stPayload.AddTarget(monster);
        stPayload.Write(StatusEffectName.Burn);
        stPayload.Write(5);
        ActionBus.Dispatch(stPayload);
                await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask BlazeBarrier(Actor target, Func<Actor, UniTask> effectPlay)
    {

        AddProtect(Battle.Instance.Player, 10);
        ClearBuff(Battle.Instance.Player, StatusEffectName.KineticVeil);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Refined, 2, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Reforge(Actor target, Func<Actor, UniTask> effectPlay)
    {

        AddProtect(Battle.Instance.Player, 5);

        StatusEffect burn = Battle.Instance.Player.Status.EffectList.GetEffect(StatusEffectName.Burn);
        if (burn != null)
            AddProtect(Battle.Instance.Player, burn.Stack * 2);

        ClearBuff(Battle.Instance.Player, StatusEffectName.KineticVeil);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Refined, 2, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Incendiary(Actor target, Func<Actor, UniTask> effectPlay)
    {

        GiveEffect(Battle.Instance.Player, StatusEffectName.LoadedIncendiary, 1, 2);
        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask HeatUp(Actor target, Func<Actor, UniTask> effectPlay)
    {

        GiveEffect(Battle.Instance.Player, StatusEffectName.Reinforce, -2, 2);
        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Overheat(Actor target, Func<Actor, UniTask> effectPlay)
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
        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Cinder(Actor target, Func<Actor, UniTask> effectPlay)
    {

        GiveEffect(Battle.Instance.Player, StatusEffectName.Searing, -2, 1);
        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask Stigma(Actor target, Func<Actor, UniTask> effectPlay)
    {
        GiveEffect(target, StatusEffectName.Branded, 1, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask OilSplash(Actor target, Func<Actor, UniTask> effectPlay)
    {
        StatusEffect burn = target.Status.EffectList.GetEffect(StatusEffectName.Burn);
        if (burn == null)
            return;

        GiveEffect(target, StatusEffectName.Burn, 0, burn.Stack);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }
    #endregion

    #region Plus
    public static async UniTask IgnitionPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        Attack(target, 8);
        GiveEffect(target, StatusEffectName.Burn, -3, 3);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask MoltenArmsPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        Attack(target, 18);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Burn, -3, 6);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask EmberPlus(Actor target, Func<Actor, UniTask> effectPlay)
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

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask InfernoPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        int count = Battle.Instance.Deck.ExtinctCardList.Count;
        Attack(target, count * 10);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask BackdraftPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        foreach (Monster monster in Battle.Instance.Monsters.List)
        {
            StatusEffect burn = monster.Status.EffectList.GetEffect(StatusEffectName.Burn);
            if (burn == null)
                continue;

            for (int i = 0; i < burn.Stack; i++)
            {

                Attack(monster, 8);
            }
        }

        ActionPayload durPayload = new()
        {
            actionId = ActorAction.GiveBuffDur,
            source = Battle.Instance.Player,
        };
        durPayload.AddTarget(Battle.Instance.Player);
        foreach (Monster monster in Battle.Instance.Monsters.List)
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
        foreach (Monster monster in Battle.Instance.Monsters.List)
            stPayload.AddTarget(monster);
        stPayload.Write(StatusEffectName.Burn);
        stPayload.Write(5);
        ActionBus.Dispatch(stPayload);
                await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask BlazeBarrierPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        AddProtect(Battle.Instance.Player, 13);
        ClearBuff(Battle.Instance.Player, StatusEffectName.KineticVeil);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Refined, 2, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask ReforgePlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        AddProtect(Battle.Instance.Player, 6);

        StatusEffect burn = Battle.Instance.Player.Status.EffectList.GetEffect(StatusEffectName.Burn);
        if (burn != null)
            AddProtect(Battle.Instance.Player, burn.Stack * 3);

        ClearBuff(Battle.Instance.Player, StatusEffectName.KineticVeil);
        ClearBuff(Battle.Instance.Player, StatusEffectName.BioActiveShell);
        GiveEffect(Battle.Instance.Player, StatusEffectName.Refined, 2, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask IncendiaryPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        GiveEffect(Battle.Instance.Player, StatusEffectName.LoadedIncendiary, 1, 4);
        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask HeatUpPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        GiveEffect(Battle.Instance.Player, StatusEffectName.Reinforce, -2, 3);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask OverheatPlus(Actor target, Func<Actor, UniTask> effectPlay)
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

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask CinderPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        GiveEffect(Battle.Instance.Player, StatusEffectName.Searing, -2, 1);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask StigmaPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {

        GiveEffect(target, StatusEffectName.Branded, 1, 2);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
    }

    public static async UniTask OilSplashPlus(Actor target, Func<Actor, UniTask> effectPlay)
    {
        StatusEffect burn = target.Status.EffectList.GetEffect(StatusEffectName.Burn);
        if (burn == null)
            return;

        GiveEffect(target, StatusEffectName.Burn, 0, burn.Stack * 2);

        await (effectPlay?.Invoke(target) ?? UniTask.CompletedTask);
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
        stPayload.Write(stack);
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