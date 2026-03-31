using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine.Events;

public class StatusEffectList
{
    private readonly Actor owner;
    private readonly Dictionary<StatusEffectName, StatusEffect> effectMap = new();
    
    public UnityEvent<IEnumerable<StatusEffect>> OnUpdate = new();

    public StatusEffectList(Actor owner)
    {
        this.owner = owner;
    }

    private StatusEffect GetOrCreate(StatusEffectName effectName)
    {
        if (!effectMap.ContainsKey(effectName))
        {
            effectMap[effectName] = StatusEffectFactory.Create(effectName, owner);
            effectMap[effectName].OnStatusChanged += HandleStatusEffectChanged;
        }

        return effectMap[effectName];
    }

    public void GiveStack(StatusEffectName effectName, int stack = 1)
    {
        GetOrCreate(effectName).GiveStack(stack);
    }   

    public void GiveDuration(StatusEffectName effectName, int duration = 1)
    {
        GetOrCreate(effectName).GiveDuration(duration);
    }

    public void RemoveStack(StatusEffectName effectName, int stack = 1)
    {
        if (!effectMap.ContainsKey(effectName)) return;

        effectMap[effectName].RemoveStack(stack);
    }

    public void RemoveDuration(StatusEffectName effectName, int duration = 1)
    {
        if (!effectMap.ContainsKey(effectName)) return;

        effectMap[effectName].RemoveDuration(duration);
    }

    public void Clear(StatusEffectName effectName)
    {
        if (!effectMap.ContainsKey(effectName)) return;

        effectMap[effectName].Clear();
    }

    public bool IsActive(StatusEffectName effectName)
    {
        if (!effectMap.ContainsKey(effectName)) return false;

        return effectMap[effectName].IsActive;
    }

    private void HandleStatusEffectChanged(StatusEffect effect)
    {
        OnUpdate.Invoke(GetActiveEffects());
    }

    public IEnumerable<StatusEffect> GetActiveEffects()
    {
        return effectMap.Values.Where(effect => effect.IsActive);
    }

    public List<T> GetActiveEffectWith<T>() where T : class
    {
        List<T> list = new();

        foreach (StatusEffect effect in effectMap.Values)
        {
            if (effect is T && effect.IsActive)
                list.Add(effect as T);
        }

        return list;
    }
}