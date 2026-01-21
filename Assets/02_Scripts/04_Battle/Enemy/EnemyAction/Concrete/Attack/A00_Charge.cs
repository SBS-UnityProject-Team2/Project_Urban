using UnityEngine;

[CreateAssetMenu(fileName = "A00_Charge", menuName = "Enemy/Actions/Attack/A00_Charge")]
public class A00_Charge : EnemyAction
{
    [SerializeField] private int damage = 6;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
    }
}