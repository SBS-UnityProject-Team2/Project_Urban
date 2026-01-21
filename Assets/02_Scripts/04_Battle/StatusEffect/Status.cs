using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class Status
{
    private readonly Target owner;

    private int attack;
    public int Attack
    {
        get
        {
            int bonus = GetEffect<Reinforce>(StatusEffectName.Reinforce)?.Stack ?? 0;
            int reduction = GetEffect<Weaken>(StatusEffectName.Weaken)?.Reduction ?? 0;
            return attack + bonus + reduction;
        }
    }

    // StatusEffect Dictionary (Lazy Initialization)
    private readonly Dictionary<StatusEffectName, StatusEffect> effectMap = new();
    public UnityEvent<IEnumerable<StatusEffect>> OnUpdate = new();

    // Typed access properties
    public Reinforce Reinforce => GetEffect<Reinforce>(StatusEffectName.Reinforce);
    public Armor Armor => GetEffect<Armor>(StatusEffectName.Armor);
    public Blur Blur => GetEffect<Blur>(StatusEffectName.Blur);
    public Refined Refined => GetEffect<Refined>(StatusEffectName.Refined);
    public LoadedIncendiary LoadedIncendiary => GetEffect<LoadedIncendiary>(StatusEffectName.LoadedIncendiary);
    public Searing Searing => GetEffect<Searing>(StatusEffectName.Searing);
    public KineticVeil KineticVeil => GetEffect<KineticVeil>(StatusEffectName.KineticVeil);
    public ElectricVeil ElectricVeil => GetEffect<ElectricVeil>(StatusEffectName.ElectricVeil);
    public Acceleration Acceleration => GetEffect<Acceleration>(StatusEffectName.Acceleration);
    public Nullification Nullification => GetEffect<Nullification>(StatusEffectName.Nullification);
    public BioActiveShell BioActiveShell => GetEffect<BioActiveShell>(StatusEffectName.BioActiveShell);
    public Regeneration Regeneration => GetEffect<Regeneration>(StatusEffectName.Regeneration);
    public ResourceTrade ResourceTrade => GetEffect<ResourceTrade>(StatusEffectName.ResourceTrade);
    public Spike Spike => GetEffect<Spike>(StatusEffectName.Spike);
    public ElasticVeil ElasticVeil => GetEffect<ElasticVeil>(StatusEffectName.ElasticVeil);

    // Debuff
    public Weaken Weaken => GetEffect<Weaken>(StatusEffectName.Weaken);
    public Broken Broken => GetEffect<Broken>(StatusEffectName.Broken);
    public Exhaust Exhaust => GetEffect<Exhaust>(StatusEffectName.Exhaust);
    public Slow Slow => GetEffect<Slow>(StatusEffectName.Slow);
    public Bleed Bleed => GetEffect<Bleed>(StatusEffectName.Bleed);
    public Burn Burn => GetEffect<Burn>(StatusEffectName.Burn);
    public Poisoned Poisoned => GetEffect<Poisoned>(StatusEffectName.Poisoned);
    public Branded Branded => GetEffect<Branded>(StatusEffectName.Branded);
    public Frozen Frozen => GetEffect<Frozen>(StatusEffectName.Frozen);
    public Anointed Anointed => GetEffect<Anointed>(StatusEffectName.Anointed);
    public Delirium Delirium => GetEffect<Delirium>(StatusEffectName.Delirium);
    public Infested Infested => GetEffect<Infested>(StatusEffectName.Infested);
    public Scarred Scarred => GetEffect<Scarred>(StatusEffectName.Scarred);
    public Dizzy Dizzy => GetEffect<Dizzy>(StatusEffectName.Dizzy);


    // 모든 효과를 생성하는 생성자 (기존 호환성 유지)
    public Status(Target owner) 
    {
        this.owner = owner;
    }

    public void IncreaseAttack(int amount)
    {
        attack += amount;
        
        OnUpdate.Invoke(GetActiveEffects());
    }

    public void DecreaseAttack(int amount)
    {
        attack -= amount;
        if (attack < 0)
            attack = 0;

        OnUpdate.Invoke(GetActiveEffects());
    }

    public StatusEffect GetEffect(StatusEffectName name)
    {
        if (!effectMap.ContainsKey(name))
        {
            var effect = StatusEffectFactory.Create(name, owner);
            effect.OnStatusChanged += HandleStatusEffectChanged;
            effectMap[name] = effect;
        }

        return effectMap[name];
    }

    private void HandleStatusEffectChanged(StatusEffect effect)
    {
        OnUpdate.Invoke(GetActiveEffects());
    }

    public T GetEffect<T>(StatusEffectName name) where T : StatusEffect
    {
        return GetEffect(name) as T;
    }

    public IEnumerable<StatusEffect> GetActiveEffects()
    {
        return effectMap.Values.Where(effect => effect.IsActive);
    }
}