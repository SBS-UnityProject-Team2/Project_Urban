using UnityEngine;

[CreateAssetMenu(fileName = "A26_CombinationAttack", menuName = "Enemy/Actions/Attack/A26_CombinationAttack")]
public class A26_CombinationAttack : EnemyAction
{
    [SerializeField] private int damage = 5;
    [SerializeField] private int count = 4;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Ruin;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}