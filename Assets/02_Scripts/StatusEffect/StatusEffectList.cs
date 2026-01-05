using System.Collections.Generic;
using UnityEngine.Events;

public class StatusEffectList
{
    private readonly List<TimedStatusEffect> effectList = new();
    private readonly Target owner;

    public StatusEffectList(Target owner)
    {
        this.owner = owner;
    }

    public UnityEvent<IEnumerable<StatusEffect>> OnUpdateList = new();
    public IEnumerable<StatusEffect> EffectList => effectList;

    public void AddEffects(TimedStatusEffect statusEffect)
    {
        statusEffect.Apply(owner);
        effectList.Add(statusEffect);
        UnityEngine.Debug.Log($"AddEffects 호출, 리스너 수: {OnUpdateList.GetPersistentEventCount()}");
        OnUpdateList?.Invoke(EffectList);
    }

    public void DecreaseTurn()
    {
        for (int i = effectList.Count - 1; i >= 0; i--)
        {
            TimedStatusEffect effect = effectList[i];
            effect.DecreaseTurn();

            if (effect.RemainingTurn <= 0)
            {
                effectList.RemoveAt(i);
                effect.Revert(owner);
            }
        }

        UnityEngine.Debug.Log($"DecreaseTurn 호출, 리스너 수: {OnUpdateList.GetPersistentEventCount()}");
        OnUpdateList?.Invoke(EffectList);
    }
}