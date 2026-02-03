using UnityEngine;

[CreateAssetMenu(fileName = "P08_DefensiveVapor", menuName = "Enemy/Actions/Protect/P08_DefensiveVapor", order = 8)]
public class P08_DefensiveVapor : EnemyAction
{
    [SerializeField] private int protectPoint = 10;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        EnemyManager.Instance.ApplyAll(enemy => enemy.Protect(protectPoint));
    }
}