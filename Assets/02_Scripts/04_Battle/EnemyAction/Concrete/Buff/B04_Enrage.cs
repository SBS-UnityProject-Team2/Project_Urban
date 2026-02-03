using UnityEngine;

[CreateAssetMenu(fileName = "B04_Enrage", menuName = "Enemy/Actions/Buff/B04_Enrage", order = 4)]
public class B04_Enrage : EnemyAction
{
    [SerializeField] private int reinforcePoint = 3;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Reinforce(reinforcePoint);
    }
}