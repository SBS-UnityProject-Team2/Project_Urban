using UnityEngine;

[CreateAssetMenu(fileName = "A02_Screw", menuName = "Enemy/Actions/Attack/A02_Screw", order = 2)]
public class A02_Screw : AttackAction
{
    [SerializeField] private int damage = 5;
    [SerializeField] private int count = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}