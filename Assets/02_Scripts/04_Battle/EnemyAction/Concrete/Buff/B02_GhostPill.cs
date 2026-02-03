using UnityEngine;

[CreateAssetMenu(fileName = "B02_GhostPill", menuName = "Enemy/Actions/Buff/B02_GhostPill", order = 0
)]
public class B02_GhostPill : EnemyAction
{
    [SerializeField] public int blurPoint = 1;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        enemy.Blur(blurPoint);
    }
}