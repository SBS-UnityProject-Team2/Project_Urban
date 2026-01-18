using UnityEngine;

[CreateAssetMenu(fileName = "B4_Enrage", menuName = "Enemy/Actions/Buff/B4_Enrage")]
public class Enrage : EnemyAction
{
    [SerializeField] private int reinforcePoint = 3;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Reinforce(reinforcePoint);
    }
}