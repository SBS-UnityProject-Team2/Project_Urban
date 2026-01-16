using System;
using System.Collections.Generic;

public static class StatusEffectFactory
{
    public static StatusEffect Create(StatusEffectName name, Target owner)
    {
        return name switch
        {
            // Buff - No owner required
            StatusEffectName.Reinforce => new Reinforce(),
            StatusEffectName.Armor => new Armor(),
            StatusEffectName.Blur => new Blur(),
            
            // Buff - Owner required
            StatusEffectName.Refined => new Refined(owner),
            StatusEffectName.LoadedIncendiary => new LoadedIncendiary(owner),
            StatusEffectName.KineticVeil => new KineticVeil(owner),
            StatusEffectName.Nullification => new Nullification(owner),
            StatusEffectName.BioActiveShell => new BioActiveShell(owner),
            StatusEffectName.Regeneration => new Regeneration(owner),
            StatusEffectName.Spike => new Spike(owner),

            // Buff - Only Player
            StatusEffectName.Acceleration => owner is Player player ? new Acceleration(player) : null,
            StatusEffectName.ElasticVeil => owner is Player player ? new ElasticVeil(player) : null,
            StatusEffectName.ResourceTrade => owner is Player player ? new ResourceTrade(player) : null,
            StatusEffectName.Searing => owner is Player player ? new Searing(player) : null,
            
            // Debuff
            StatusEffectName.Weaken => new Weaken(owner),
            StatusEffectName.Broken => new Broken(owner),
            StatusEffectName.Bleed => new Bleed(owner),
            StatusEffectName.Burn => new Burn(owner),
            StatusEffectName.Poisoned => new Poisoned(owner),
            StatusEffectName.Branded => new Branded(owner),
            StatusEffectName.Frozen => new Frozen(owner),
            StatusEffectName.Anointed => new Anointed(owner),
            StatusEffectName.Delirium => new Delirium(owner),
            StatusEffectName.Infested => new Infested(owner),
            StatusEffectName.Scarred => new Scarred(owner),

            StatusEffectName.Dizzy =>  owner is Player player ? new Dizzy(player) : null,
            StatusEffectName.Exhaust =>  owner is Player player ? new Exhaust(player) : null,
            StatusEffectName.Slow =>  owner is Player player ? new Slow(player) : null,
            
            _ => throw new ArgumentException($"Unknown StatusEffectName: {name}")
        };
    }

    public static Dictionary<StatusEffectName, StatusEffect> CreateEffects(IEnumerable<StatusEffectName> effectNames, Target owner)
    {
        var effectMap = new Dictionary<StatusEffectName, StatusEffect>();
        
        foreach (var name in effectNames)
        {
            effectMap[name] = Create(name, owner);
        }
        
        return effectMap;
    }
}
