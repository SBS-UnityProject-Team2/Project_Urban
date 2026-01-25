using UnityEngine;

[CreateAssetMenu(fileName = "P01_Smoke", menuName = "Enemy/Actions/Protect/P01_Smoke")]
public class P01_Smoke : EnemyAction
{
    [SerializeField] private int protectPoint = 10;
    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
    }
}