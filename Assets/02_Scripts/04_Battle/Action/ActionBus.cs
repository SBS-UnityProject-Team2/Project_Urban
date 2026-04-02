#pragma warning disable IDE0051

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Cysharp.Threading.Tasks;

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
    MoveCardFromDeck,
    MoveCardFromHand,
    MoveCardFromDiscard,
    MoveCardFromExhaust,
    SearchCard,
    MoveSelectedCard,
    DrawCount,
    // 코스트 조작 관련
    SetCardCost = 50208,
    AddCardCost,
    ReduceCardCost,
    RandomizeCardCost,
    ResetCardCost,
    // 카드 생성 관련
    CreateCard = 50213,
    CopyCard,
    TransformCard,
    ResetCardTransform,
    // 버프 디버프 관련
    GiveBuffDur = 50301,
    GiveBuffSta,
    RemoveBuffDur,
    RemoveBuffSta,
    ClearBuffs,
    CancelBuff,
    ActionSkip,
}

public static class ActionBus
{
    private static readonly float damageModifier = 0.3f;
    
    private static Queue<ActionPayload> queue = new();

    private static readonly Dictionary<ActorAction, Action<ActionPayload>> dispatchTable;

    static ActionBus()
    {
        dispatchTable = new();

        MethodInfo[] methods = typeof(ActionBus).GetMethods(BindingFlags.NonPublic | BindingFlags.Static);

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

    #region 공격
    private static void AtkDmg(ActionPayload payload)
    {
        ElementType element = payload.Read<ElementType>();
        int baseDamage = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            DamageIncomingPayload incomingPayload = new()
            {
                source = payload.source,
                target = target,
            };
            target.DispatchEvent(incomingPayload);

            if (TryDamageNullification(target))
                return;

            int damageByElement = ModifyDamageByElement(baseDamage, target.Status.Element.Type, element);
            int damageByStatusEffect = ModifyStatusEffect(target, payload.source, damageByElement, element);
            int attackFlatDelta = ModifyAttackerDamageChange(payload.source, damageByStatusEffect, element);

            Health targetHealth = target.Status.Health;
            int remainingDamage = attackFlatDelta;
            if (targetHealth.Block > 0)
            {
                int absorbedDamage = Math.Min(targetHealth.Block, remainingDamage);
                remainingDamage -= absorbedDamage;
                targetHealth.Block -= absorbedDamage;

                if (targetHealth.Block == 0)
                {
                    BreakPayload breakPayload = new()
                    {
                        source = payload.source,
                        target = target,
                    };

                    target.DispatchEvent(incomingPayload);
                }
            }

            targetHealth.CurHp -= remainingDamage;
            AttackDamageTakenPayload attackDamageTakenPayload = new()
            {
                source = payload.source,
                target = target,
                damage = remainingDamage
            };
            target.DispatchEvent(attackDamageTakenPayload);

            if (targetHealth.CurHp == 0)
            {
                DeadPayload deadPayload = new()
                {
                    source = payload.source,
                    target = target,
                };
                target.DispatchEvent(deadPayload);
            }
        });
    }

    private static void AtkFixedDmg(ActionPayload payload)
    {
        ElementType element = payload.Read<ElementType>();
        int baseDamage = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            DamageIncomingPayload incomingPayload = new()
            {
                source = payload.source,
                target = target,
            };
            target.DispatchEvent(incomingPayload);

            if (TryDamageNullification(target))
            {
                // 데미지 무효화 처리
            }

            int damageByElement = ModifyDamageByElement(baseDamage, target.Status.Element.Type, element);
            int damageByStatusEffect = ModifyStatusEffect(target, payload.source, damageByElement, element);
            int attackFlatDelta = ModifyAttackerDamageChange(payload.source, damageByStatusEffect, element);

            Health targetHealth = target.Status.Health;
            targetHealth.CurHp -= attackFlatDelta;
            AttackDamageTakenPayload attackDamageTakenPayload = new()
            {
                source = payload.source,
                target = target,
                damage = attackFlatDelta
            };
            target.DispatchEvent(attackDamageTakenPayload);

            if (targetHealth.CurHp == 0)
            {
                DeadPayload deadPayload = new()
                {
                    source = payload.source,
                    target = target,
                };
                target.DispatchEvent(deadPayload);
            }
        });
    }

    private static void AtkLossHp(ActionPayload payload)
    {
        int damage = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            Health targetHealth = target.Status.Health;
            targetHealth.CurHp -= damage;

            EffectDamageTakenPayload effectDamageTakenPayload = new()
            {
                source = payload.source,
                target = target,
                damage = damage
            };
            target.DispatchEvent(effectDamageTakenPayload);

            if (targetHealth.CurHp == 0)
            {
                DeadPayload deadPayload = new()
                {
                    source = payload.source,
                    target = target,
                };
                target.DispatchEvent(deadPayload);
            }
        });
    }

    private static void DmgAdjust(ActionPayload payload)
    {
        int maxDamage = payload.Read<int>();
    }

    private static void DmgRateAdjust(ActionPayload payload)
    {
        float maxDamageRate = payload.Read<float>();
    }

    #endregion

    #region 방어 및 지원
    private static void AddBlock(ActionPayload payload)
    {
        int block = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            target.Status.Health.Block += ModifyBlockChange(target, block);

            ProtectPayload protectPayload = new()
            {
                source = payload.source,
                target = target,
                blockPoint = block
            };
            target.DispatchEvent(protectPayload);
        });
    }

    private static void HealHp(ActionPayload payload)
    {
        int healPoint = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            target.Status.Health.CurHp += ModifyHealChange(target, healPoint);

            UpdateCurHpPayload updateCurHpPayload = new()
            {
                source = payload.source,
                target = target,
                updateDelta = healPoint
            };
            target.DispatchEvent(updateCurHpPayload);
        });
    }

    private static void AddMaxHp(ActionPayload payload)
    {
        int maxHpPoint = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            target.Status.Health.MaxHp += maxHpPoint;

            UpdateMaxHpPayload updateMaxHpPayload = new()
            {
                source = payload.source,
                target = target,
                updateDelta = maxHpPoint
            };
            target.DispatchEvent(updateMaxHpPayload);
        });
    }

    private static void AddCost(ActionPayload payload)
    {
        int costPoint = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            target.Status.Cost.CurCost += costPoint;

            UpdateCurCostPayload updateCurCostPayload = new()
            {
                source = payload.source,
                target = target,
                updateDelta = costPoint
            };
            target.DispatchEvent(updateCurCostPayload);
        });
    }

    private static void ChangeElement(ActionPayload payload)
    {
        ElementType elementType = payload.Read<ElementType>();

        payload.targets.ForEach(target =>
        {
            ElementType before = target.Status.Element.Type;
            target.Status.Element.ChangeType(elementType);

            UpdateElementPayload updateElementPayload = new()
            {
                source = payload.source,
                target = target,
                beforeType = before,
                afterType = elementType
            };
            target.DispatchEvent(updateElementPayload);
        });
    }

    private static void TakenDmgRateAdjust(ActionPayload payload)
    {
        float damageRate = payload.Read<float>();
    }
    #endregion

    #region 카드 조작
    private static void ShuffleDeck(ActionPayload payload)
    {
        Battle.Instance.Deck.Shuffle();
    }

    private static void MoveCardFromDeck(ActionPayload payload)
    {
        Location to = payload.Read<Location>();
        int cardCount = payload.Read<int>();

        Battle.Instance.Deck.MoveCard(Location.Deck, to, cardCount).Forget();
    }

    private static void MoveCardFromHand(ActionPayload payload)
    {
        Location to = payload.Read<Location>();
        int cardCount = payload.Read<int>();

        Battle.Instance.Deck.MoveCard(Location.Hand, to, cardCount).Forget();
    }

    private static void MoveCardFromDiscard(ActionPayload payload)
    {
        Location to = payload.Read<Location>();
        int cardCount = payload.Read<int>();

        Battle.Instance.Deck.MoveCard(Location.DiscardPile, to, cardCount).Forget();
    }

    private static void MoveCardFromExhaust(ActionPayload payload)
    {
        Location to = payload.Read<Location>();
        int cardCount = payload.Read<int>();

        Battle.Instance.Deck.MoveCard(Location.ExhaustPile, to, cardCount).Forget();
    }

    private static void SearchCard(ActionPayload payload)
    {
        int cardCount = payload.Read<int>();
    }

    private static void MoveSelectedCard(ActionPayload payload)
    {
    }

    private static void DrawCount(ActionPayload payload)
    {
        int drawCount = payload.Read<int>();

        Battle.Instance.Deck.DrawCard(drawCount).Forget();
        
        DrawPayload drawPayload = new()
        {
            source = payload.source,  
            target = payload.targets[0],
            drawCount = drawCount
        };
        payload.targets[0].DispatchEvent(drawPayload);
    }
    #endregion

    #region 코스트 조작
    private static void SetCardCost(ActionPayload payload)
    {
        int cardInstanceId = payload.Read<int>();
        int costPoint = payload.Read<int>();

        Card card = Battle.Instance.Deck.Hand.GetCard(cardInstanceId);
        card.SetCost(costPoint);
    }

    private static void AddCardCost(ActionPayload payload)
    {
        int cardInstanceId = payload.Read<int>();
        int costPoint = payload.Read<int>();

        Card card = Battle.Instance.Deck.Hand.GetCard(cardInstanceId);
        card.AddCost(costPoint);
    }

    private static void ReduceCardCost(ActionPayload payload)
    {
        int cardInstanceId = payload.Read<int>();
        int costPoint = payload.Read<int>();

        Card card = Battle.Instance.Deck.Hand.GetCard(cardInstanceId);
        card.ReduceCost(costPoint);
    }

    private static void RandomizeCardCost(ActionPayload payload)
    {
        int cardInstanceId = payload.Read<int>();
        int maxCostPoint = payload.Read<int>();

        Card card = Battle.Instance.Deck.Hand.GetCard(cardInstanceId);
        card.SetCost(UnityEngine.Random.Range(0, maxCostPoint + 1));
    }

    private static void ResetCardCost(ActionPayload payload)
    {
        int cardInstanceId = payload.Read<int>();

        Card card = Battle.Instance.Deck.Hand.GetCard(cardInstanceId);
        card.ResetCost();
    }

    #endregion

    #region 카드 생성
    private static void CreateCard(ActionPayload payload)
    {
        Location to = payload.Read<Location>();
        CardName cardName = payload.Read<CardName>();

        Battle.Instance.Deck.CreateCard(cardName, to);
    }

    private static void CopyCard(ActionPayload payload)
    {
        Location to = payload.Read<Location>();
        CardName cardName = payload.Read<CardName>();

        Battle.Instance.Deck.CreateCard(cardName, to);
    }

    private static void TransformCard(ActionPayload payload)
    {
        int cardId = payload.Read<int>();
        CardName to = payload.Read<CardName>();

        Card card = Battle.Instance.Deck.GetCard(cardId);
        card.Transform(to);
    }

    private static void ResetCardTransform(ActionPayload payload)
    {
        int cardId = payload.Read<int>();

        Card card = Battle.Instance.Deck.GetCard(cardId);
        card.ResetTransform();
    }
    #endregion

    #region 버프/디버프
    private static void GiveBuffDur(ActionPayload payload)
    {
        StatusEffectName effectName = payload.Read<StatusEffectName>();
        int duration = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            target.Status.EffectList.GiveDuration(effectName, duration);

            GiveEffectPayload giveEffectPayload = new()
            {
                source = payload.source,
                target = target,
                effectName = effectName,
                duration = duration
            };
            target.DispatchEvent(giveEffectPayload);
        });
    }

    private static void GiveBuffSta(ActionPayload payload)
    {
        StatusEffectName effectName = payload.Read<StatusEffectName>();
        int stack = payload.Read<int>();

        payload.targets.ForEach(target => 
        {
            target.Status.EffectList.GiveStack(effectName, stack);

            GiveEffectPayload giveEffectPayload = new()
            {
                source = payload.source,
                target = target,
                effectName = effectName,
                stack = stack
            };
            target.DispatchEvent(giveEffectPayload);    
        });
    }

    private static void RemoveBuffDur(ActionPayload payload)
    {
        StatusEffectName effectName = payload.Read<StatusEffectName>();
        int duration = payload.Read<int>();

        payload.targets.ForEach(target => 
        {
            target.Status.EffectList.RemoveDuration(effectName, duration);

            RemoveEffectPayload removeEffectPayload = new()
            {
                source = payload.source,
                target = target,
                effectName = effectName,
                duration = duration
            };
            target.DispatchEvent(removeEffectPayload);
        });
    }

    private static void RemoveBuffSta(ActionPayload payload)
    {
        StatusEffectName effectName = payload.Read<StatusEffectName>();
        int stack = payload.Read<int>();

        payload.targets.ForEach(target =>
        {
            target.Status.EffectList.RemoveStack(effectName, stack);

            RemoveEffectPayload removeEffectPayload = new()
            {
                source = payload.source,
                target = target,
                effectName = effectName,
                stack = stack
            };
            target.DispatchEvent(removeEffectPayload);
        });
    }

    private static void ClearBuffs(ActionPayload payload)
    {
        StatusEffectName effectName = payload.Read<StatusEffectName>();

        payload.targets.ForEach(target =>
        {
            target.Status.EffectList.Clear(effectName);

            ClearEffectPayload clearEffectPayload = new()
            {
                source = payload.source,
                target = target,
                effectName = effectName,
            };
            target.DispatchEvent(clearEffectPayload);
        });
    }

    private static void CancelBuff(ActionPayload payload)
    {
        StatusEffectName before = payload.Read<StatusEffectName>();
        StatusEffectName after = payload.Read<StatusEffectName>();
    }

    private static void ActionSkip(ActionPayload payload)
    {
    }
    #endregion

    #region Helper Methods
    private static int ModifyDamageByElement(int hitPoint, ElementType blockType, ElementType attackType)
    {
        if (blockType == ElementType.None) return hitPoint;

        int modifiedDamage = 0;

        if (attackType == ElementType.Ruin)
        {
            if (blockType == ElementType.Bio)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (blockType == ElementType.Psychic)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        else if (attackType == ElementType.Psychic)
        {
            if (blockType == ElementType.Ruin)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (blockType == ElementType.Bio)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        else if (attackType == ElementType.Bio)
        {
            if (blockType == ElementType.Psychic)
                modifiedDamage = (int)(hitPoint * damageModifier);

            if (blockType == ElementType.Ruin)
                modifiedDamage = (int)(-hitPoint * damageModifier);
        }

        return hitPoint + modifiedDamage;
    }

    private static int ModifyStatusEffect(Actor defender, Actor attacker, int damage, ElementType attackType)
    {
        float rateDelta = 0f;

        List<IDefenderDamageRateChange> defenderRateChanges = defender.Status.EffectList.GetActiveEffectWith<IDefenderDamageRateChange>();
        foreach (IDefenderDamageRateChange defenderRateChange in defenderRateChanges)
            rateDelta += defenderRateChange.GetDamageDelta(attackType);

        List<IAttackerDamageRateChange> attackerRateChanges = attacker.Status.EffectList.GetActiveEffectWith<IAttackerDamageRateChange>();
        foreach (IAttackerDamageRateChange attackerRateChange in attackerRateChanges)
            rateDelta += attackerRateChange.GetDamageDelta();

        float rateMultiplier = Math.Max(0f, 1f + rateDelta);
        return (int)(damage * rateMultiplier);
    }

    private static int ModifyAttackerDamageChange(Actor attacker, int damage, ElementType attackType)
    {
        List<IAttackerDamageFlatChange> attackChanges = attacker.Status.EffectList.GetActiveEffectWith<IAttackerDamageFlatChange>();

        int attackFlatDelta = 0;

        foreach (IAttackerDamageFlatChange attackChange in attackChanges)
            attackFlatDelta += attackChange.GetDamageDelta();

        return attackFlatDelta;
    }

    private static bool TryDamageNullification(Actor defender)
    {
        List<IDamageNullifier> nullifiers = defender.Status.EffectList.GetActiveEffectWith<IDamageNullifier>();

        bool isNullified = false;

        foreach (IDamageNullifier nullifier in nullifiers)
            isNullified |= nullifier.TryNullification();

        return isNullified;
    }

    private static int ModifyHealChange(Actor actor, int healPoint)
    {
        List<IHealChange> healChanges = actor.Status.EffectList.GetActiveEffectWith<IHealChange>();

        int baseHeal = healPoint;
        int modifiedHeal = baseHeal;

        foreach (IHealChange healChange in healChanges)
            modifiedHeal += healChange.GetHealDelta(baseHeal);

        return Math.Max(0, modifiedHeal);
    }

    private static int ModifyBlockChange(Actor actor, int blockPoint)
    {
        List<IBlockChange> blockChanges = actor.Status.EffectList.GetActiveEffectWith<IBlockChange>();

        int baseBlock = blockPoint;
        int modifiedBlock = baseBlock;

        foreach (IBlockChange blockChange in blockChanges)
            modifiedBlock += blockChange.GetBlockDelta();

        return Math.Max(0, modifiedBlock);
    }
    #endregion
}

