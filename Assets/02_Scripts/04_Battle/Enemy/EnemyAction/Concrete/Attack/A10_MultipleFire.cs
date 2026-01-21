using UnityEngine;

[CreateAssetMenu(fileName = "A10_MultipleFire", menuName = "Enemy/Actions/Attack/A10_MultipleFire")]
public class A10_MultipleFire : EnemyAction
{
    [SerializeField] private int damage = 3;
    [SerializeField] private int count = 5;


    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}