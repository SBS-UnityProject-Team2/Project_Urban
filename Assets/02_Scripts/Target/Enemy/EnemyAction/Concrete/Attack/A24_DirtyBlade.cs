using UnityEngine;

[CreateAssetMenu(fileName = "A24_DirtyBlade", menuName = "Enemy/Actions/Attack/A24_DirtyBlade")]
public class A24_DirtyBlade : EnemyAction
{
    [SerializeField] private int damage = 12;
    [SerializeField] private int count = 1;
    [SerializeField] private int poisonedPoint = 1;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Bio;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Poisoned(poisonedPoint);
    }
}