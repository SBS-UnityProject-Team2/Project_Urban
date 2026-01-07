using UnityEngine;

[CreateAssetMenu(fileName = "A7_RepulsiveMatrix", menuName = "Enemy/Actions/Attack/A7_RepulsiveMatrix")]
public class RepulsiveMatrix : EnemyAction
{
    [SerializeField] private int damage = 9;
    [SerializeField] private int protect = 9;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Ice;

    public override void Execute(Target target)
    {
        // target.Damage(damage);
        target.Protect(protect);   
    }
}