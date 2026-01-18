using UnityEngine;

[CreateAssetMenu(fileName = "A02_Screw", menuName = "Enemy/Actions/Attack/A02_Screw")]
public class A02_Screw : EnemyAction
{
    [SerializeField] private int damage = 5;
    [SerializeField] private int count = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}