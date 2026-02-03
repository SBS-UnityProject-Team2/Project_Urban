using UnityEngine;

[CreateAssetMenu(fileName = "B00_Sharpening", menuName = "Enemy/Actions/Buff/B00_Sharpening", order = 0
)]
public class B00_Sharpening : EnemyAction
{
    [SerializeField] private int burstPoint = 9;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        enemy.Burst(burstPoint);
    }
}