using System;
using System.Collections.Generic;
using UnityEngine.Events;

public static class CardMethods
{
    private static readonly Dictionary<CardName, (UnityAction<Actor> normal, UnityAction<Actor> plus)> methodMap = new();

    public static void Dispatch(CardName cardName, Actor target, bool isEnchanted = false)
    {
        if (!isEnchanted) methodMap[cardName].normal(target);
        else methodMap[cardName].plus(target);
    }

    static CardMethods()
    {
        // Physical
        methodMap[CardName.Punch] = (Physical.Punch, Physical.PunchPlus);
        methodMap[CardName.Shooting] = (Physical.Shooting, Physical.ShootingPlus);
        methodMap[CardName.Strike] = (Physical.Strike, Physical.StrikePlus);
        methodMap[CardName.VileAttack] = (Physical.VileAttack, Physical.VileAttackPlus);
        methodMap[CardName.Assault] = (Physical.Assault, Physical.AssaultPlus);
        methodMap[CardName.Guard] = (Physical.Guard, Physical.GuardPlus);
        methodMap[CardName.Rollout] = (Physical.Rollout, Physical.RolloutPlus);
        methodMap[CardName.Maintenance] = (Physical.Maintenance, Physical.MaintenancePlus);
        methodMap[CardName.Dummy] = (Physical.Dummy, Physical.DummyPlus);

        // Ruin
        methodMap[CardName.Ignition] = (Ruin.Ignition, Ruin.IgnitionPlus);
        methodMap[CardName.MoltenArms] = (Ruin.MoltenArms, Ruin.MoltenArmsPlus);
        methodMap[CardName.Ember] = (Ruin.Ember, Ruin.EmberPlus);
        methodMap[CardName.Inferno] = (Ruin.Inferno, Ruin.InfernoPlus);
        methodMap[CardName.Backdraft] = (Ruin.Backdraft, Ruin.BackdraftPlus);
        methodMap[CardName.BlazeBarrier] = (Ruin.BlazeBarrier, Ruin.BlazeBarrierPlus);
        methodMap[CardName.Reforge] = (Ruin.Reforge, Ruin.ReforgePlus);
        methodMap[CardName.Incendiary] = (Ruin.Incendiary, Ruin.IncendiaryPlus);
        methodMap[CardName.HeatUp] = (Ruin.HeatUp, Ruin.HeatUpPlus);
        methodMap[CardName.Overheat] = (Ruin.Overheat, Ruin.OverheatPlus);
        methodMap[CardName.Cinder] = (Ruin.Cinder, Ruin.CinderPlus);
        methodMap[CardName.Stigma] = (Ruin.Stigma, Ruin.StigmaPlus);
        methodMap[CardName.OilSplash] = (Ruin.OilSplash, Ruin.OilSplashPlus);

        // Psychic
        methodMap[CardName.GlacierWedge] = (Psychic.GlacierWedge, Psychic.GlacierWedgePlus);
        methodMap[CardName.FlowArrow] = (Psychic.FlowArrow, Psychic.FlowArrowPlus);
        methodMap[CardName.EnergyNeedle] = (Psychic.EnergyNeedle, Psychic.EnergyNeedlePlus);
        methodMap[CardName.Pulse] = (Psychic.Pulse, Psychic.PulsePlus);
        methodMap[CardName.KineticGrasp] = (Psychic.KineticGrasp, Psychic.KineticGraspPlus);
        methodMap[CardName.IceShield] = (Psychic.IceShield, Psychic.IceShieldPlus);
        methodMap[CardName.ElectricField] = (Psychic.ElectricField, Psychic.ElectricFieldPlus);
        methodMap[CardName.AccelConcoction] = (Psychic.AccelConcoction, Psychic.AccelConcoctionPlus);
        methodMap[CardName.SuperConducter] = (Psychic.SuperConducter, Psychic.SuperConducterPlus);
        methodMap[CardName.Anxiolytic] = (Psychic.Anxiolytic, Psychic.AnxiolyticPlus);
        methodMap[CardName.CryoPowder] = (Psychic.CryoPowder, Psychic.CryoPowderPlus);
        methodMap[CardName.Disturb] = (Psychic.Disturb, Psychic.DisturbPlus);

        // Bio
        methodMap[CardName.DoubleEdge] = (Bio.DoubleEdge, Bio.DoubleEdgePlus);
        methodMap[CardName.Plague] = (Bio.Plague, Bio.PlaguePlus);
        methodMap[CardName.ThornWhip] = (Bio.ThornWhip, Bio.ThornWhipPlus);
        methodMap[CardName.AbsorbingStrike] = (Bio.AbsorbingStrike, Bio.AbsorbingStrikePlus);
        methodMap[CardName.DistortedSlay] = (Bio.DistortedSlay, Bio.DistortedSlayPlus);
        methodMap[CardName.SpikyBush] = (Bio.SpikyBush, Bio.SpikyBushPlus);
        methodMap[CardName.ElasticWall] = (Bio.ElasticWall, Bio.ElasticWallPlus);
        methodMap[CardName.Blooming] = (Bio.Blooming, Bio.BloomingPlus);
        methodMap[CardName.SurgingLife] = (Bio.SurgingLife, Bio.SurgingLifePlus);
        methodMap[CardName.CellChange] = (Bio.CellChange, Bio.CellChangePlus);
        methodMap[CardName.Cycle] = (Bio.Cycle, Bio.CyclePlus);
        methodMap[CardName.EnfeebleSludge] = (Bio.EnfeebleSludge, Bio.EnfeebleSludgePlus);
    }
}