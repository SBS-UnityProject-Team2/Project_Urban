using System;
using UnityEngine;

[Flags]
public enum ActionType
{
    Unknown = 0,
    Attack = 1,  // 공격
    Protect = 1 << 1, // 방어
    Buff = 1 << 2,    // 버프
    Debuff = 1 << 3,   // 디버프
    Stun = 1 << 4,
}


abstract public class EnemyAction : ScriptableObject
{
    abstract public ActionType Type { get; }
    abstract public Element Element { get; }

    abstract public void Execute(Enemy enemy, Target target);
} 

abstract public class AttackAction : EnemyAction
{
    abstract public int Damage { get; }
    abstract public int Count { get; } 
}