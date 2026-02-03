using UnityEngine;

[CreateAssetMenu(fileName = "A01_Bite", menuName = "Enemy/Actions/Attack/A01_Bite", order = 1)]
public class A01_Bite : AttackAction
{
    [SerializeField] private int damage = 6;
    [SerializeField] private int count = 1;
    [SerializeField] private int bleedPoint = 5;
    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.None;
    public override int Damage => damage;
    public override int Count => count;
    
    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
        target.Bleed(bleedPoint);
    }
}