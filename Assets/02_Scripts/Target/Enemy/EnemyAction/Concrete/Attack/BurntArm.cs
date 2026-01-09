using UnityEngine;

[CreateAssetMenu(fileName = "A5_BurntArm", menuName = "Enemy/Actions/Attack/A5_BurntArm")]
public class BurntArm : EnemyAction
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int reinforce = 3;

    public override ActionType Type => ActionType.Attack;

    public override Element Element => Element.Ruin;

    public override void Execute(Target target)
    {
        // target.Damage(damage);
        target.Reinforce(reinforce);
    }
}