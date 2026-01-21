using UnityEngine;

public enum ActionType
{
    Attack,  // 공격
    Protect, // 방어
    Buff,    // 버프
    Debuff   // 디버프
}


abstract public class EnemyAction : ScriptableObject
{
    abstract public ActionType Type { get; }
    abstract public Element Element { get; }

    abstract public void Execute(Enemy enemy, Target target);
} 