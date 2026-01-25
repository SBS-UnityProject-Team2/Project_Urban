using UnityEngine;

[CreateAssetMenu(fileName = "A16_HeavyAttack", menuName = "Enemy/Actions/Attack/A16_HeavyAttack")]
public class A16_HeavyAttack : EnemyAction
{
    [SerializeField] private int damage = 22;
    [SerializeField] private int count = 1;
    [SerializeField] private int brokenPoint = 2;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Ruin;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        enemy.Broken(brokenPoint);
    }
}