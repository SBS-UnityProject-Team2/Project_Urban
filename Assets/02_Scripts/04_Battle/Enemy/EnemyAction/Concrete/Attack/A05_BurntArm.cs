using UnityEngine;

[CreateAssetMenu(fileName = "A05_BurntArm", menuName = "Enemy/Actions/Attack/A05_BurntArm")]
public class A05_BurntArm : EnemyAction
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int reinforcePoint = 3;

    public override ActionType Type => ActionType.Attack;

    public override Element Element => Element.Ruin;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
        enemy.Reinforce(reinforcePoint);
    }
}