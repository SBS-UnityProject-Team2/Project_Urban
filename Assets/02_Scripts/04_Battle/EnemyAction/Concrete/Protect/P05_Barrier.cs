using UnityEngine;

[CreateAssetMenu(fileName = "P05_Barrier", menuName = "Enemy/Actions/Protect/P05_Barrier", order = 5)]
public class P05_Barrier : EnemyAction
{
    [SerializeField] private int protectPoint = 12;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
    }
}