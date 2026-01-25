using UnityEngine;

[CreateAssetMenu(fileName = "P06_Rush", menuName = "Enemy/Actions/Protect/P06_Rush")]
public class P06_Rush : EnemyAction
{
    [SerializeField] private int protectPoint = 12;
    [SerializeField] private int reinforcePoint = 2;

    public override ActionType Type => ActionType.Protect | ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
        target.Reinforce(reinforcePoint);
    }
}