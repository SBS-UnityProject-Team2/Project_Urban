using UnityEngine;

[CreateAssetMenu(fileName = "A09_CorrosiveShock", menuName = "Enemy/Actions/Attack/A09_CorrosiveShock")]
public class A09_CorrosiveShock : EnemyAction
{
    [SerializeField] private int damage = 7;
    [SerializeField] private int count = 2;
    [SerializeField] private int brokenPoint = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Bio;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
            
        target.Broken(brokenPoint);
    }
}