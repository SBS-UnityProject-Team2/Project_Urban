using UnityEngine;

[CreateAssetMenu(fileName = "P00_Curl", menuName = "Enemy/Actions/Protect/P00_Curl")]
public class P00_Curl : EnemyAction
{
    [SerializeField] private int protectPoint = 7;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
    }
}