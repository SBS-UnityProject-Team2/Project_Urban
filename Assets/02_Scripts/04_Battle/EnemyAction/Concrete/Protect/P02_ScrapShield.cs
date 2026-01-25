using UnityEngine;

[CreateAssetMenu(fileName = "P02_ScrapShield", menuName = "Enemy/Actions/Protect/P02_ScrapShield")]
public class P02_ScrapShield : EnemyAction
{
    [SerializeField] private int protectPoint = 15;
    [SerializeField] private int brokenPoint = 2;

    public override ActionType Type => ActionType.Protect | ActionType.Debuff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
        target.Broken(brokenPoint);
    }
}