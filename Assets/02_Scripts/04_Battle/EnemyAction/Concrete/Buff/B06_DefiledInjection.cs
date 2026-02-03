using UnityEngine;

[CreateAssetMenu(fileName = "B06_DefiledInjection", menuName = "Enemy/Actions/Buff/B06_DefiledInjection", order = 6)]
public class B06_DefiledInjection : EnemyAction
{
    [SerializeField] private int reinforcePoint = 4;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        enemy.Reinforce(reinforcePoint);
        enemy.Element = Element.Bio;
    }
}