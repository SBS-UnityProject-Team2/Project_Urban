using UnityEngine;

[CreateAssetMenu(fileName = "A0_Charge", menuName = "Enemy/Actions/Attack/A0_Charge")]
public class Charge : EnemyAction
{
    [SerializeField] private int damage = 6;

    public override ActionType Type => ActionType.Attack;

    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Damage(damage);
    }
}