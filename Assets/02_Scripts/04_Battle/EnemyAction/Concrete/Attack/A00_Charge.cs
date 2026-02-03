using UnityEngine;

[CreateAssetMenu(fileName = "A00_Charge", menuName = "Enemy/Actions/Attack/A00_Charge", order = 0)]
public class A00_Charge : AttackAction
{
    [SerializeField] private int damage = 6;
    [SerializeField] private int count = 1;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
    }
}