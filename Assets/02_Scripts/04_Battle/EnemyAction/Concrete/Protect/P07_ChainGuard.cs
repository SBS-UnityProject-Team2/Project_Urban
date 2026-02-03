using UnityEngine;

[CreateAssetMenu(fileName = "P07_ChainGuard", menuName = "Enemy/Actions/Protect/P07_ChainGuard", order = 7)]
public class P07_ChainGuard : EnemyAction
{
    [SerializeField] private int protectPoint = 14;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
    }
}