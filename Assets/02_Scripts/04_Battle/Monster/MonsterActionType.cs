using System;

[Flags]
public enum MonsterActionType
{
    None    = 0,
    Attack  = 1 << 0,
    Defense = 1 << 1,
    Buff    = 1 << 2,
    Debuff  = 1 << 3,
    Escape  = 1 << 4,
    Stun    = 1 << 5,
    Heal    = 1 << 6,
    Summon  = 1 << 7,
    Unknown = 1 << 8
}