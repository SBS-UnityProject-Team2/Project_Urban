using System;

public static class StatusEffectFactory
{
    public static StatusEffect Create(StatusEffectName name, Actor owner)
    {
        return name switch
        {
            // Buff
            StatusEffectName.Reinforce => new Reinforce(owner),
            StatusEffectName.Armor => new Armor(owner),
            StatusEffectName.Preparation => new Preparation(owner),
            StatusEffectName.Blur => new Blur(owner),
            StatusEffectName.Refined => new Refined(owner),
            StatusEffectName.LoadedIncendiary => new LoadedIncendiary(owner),
            StatusEffectName.Searing => new Searing(owner),
            StatusEffectName.KineticVeil => new KineticVeil(owner),
            StatusEffectName.ElectricVeil => new ElectricVeil(owner),
            StatusEffectName.Acceleration => new Acceleration(owner),
            StatusEffectName.Nullification => new Nullification(owner),
            StatusEffectName.BioActiveShell => new BioActiveShell(owner),
            StatusEffectName.Regeneration => new Regeneration(owner),
            StatusEffectName.ResourceTrade => new ResourceTrade(owner),
            StatusEffectName.Spike => new Spike(owner),
            StatusEffectName.ElasticVeil => new ElasticVeil(owner),
            StatusEffectName.Burst => new Burst(owner),
            StatusEffectName.Coating => new Coating(owner),
            StatusEffectName.FrozenResistance => new FrozenResistance(owner),

            // Debuff
            StatusEffectName.Weaken => new Weaken(owner),
            StatusEffectName.Broken => new Broken(owner),
            StatusEffectName.Exhaust => new Exhaust(owner),
            StatusEffectName.Slow => new Slow(owner),
            StatusEffectName.Bleed => new Bleed(owner),
            StatusEffectName.Burn => new Burn(owner),
            StatusEffectName.Poisoned => new Poisoned(owner),
            StatusEffectName.Branded => new Branded(owner),
            StatusEffectName.Frost => new Frost(owner),
            StatusEffectName.Frozen => new Frozen(owner),
            StatusEffectName.Anointed => new Anointed(owner),
            StatusEffectName.Delirium => new Delirium(owner),
            StatusEffectName.Infested => new Infested(owner),
            StatusEffectName.Scarred => new Scarred(owner),
            StatusEffectName.Dizzy => new Dizzy(owner),
            StatusEffectName.Summoned => new Summoned(owner),
            StatusEffectName.FocusStance => new FocusStance(owner), 
            
            _ => throw new ArgumentException($"Unknown StatusEffectName: {name}")
        };
    }
} 