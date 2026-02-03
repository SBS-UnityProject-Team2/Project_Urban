using UnityEngine;

[CreateAssetMenu(fileName = "A05_BurntArm", menuName = "Enemy/Actions/Attack/A05_BurntArm", order = 5)]
public class A05_BurntArm : AttackAction
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int count = 1;
    [SerializeField] private int reinforcePoint = 3;

    public override ActionType Type => ActionType.Attack | ActionType.Buff;

    public override Element Element => Element.Ruin;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
        enemy.Reinforce(reinforcePoint);
    }
}