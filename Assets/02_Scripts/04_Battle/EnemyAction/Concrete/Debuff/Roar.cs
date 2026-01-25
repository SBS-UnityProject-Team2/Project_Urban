using UnityEngine;

[CreateAssetMenu(fileName = "D0_Roar", menuName = "Enemy/Actions/Debuff/D0_Roar")]
public class Roar : EnemyAction
{
    [SerializeField] private int weakenPoint;
    public override ActionType Type => ActionType.Debuff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Weaken(weakenPoint);
    }
}