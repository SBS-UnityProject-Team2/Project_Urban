using UnityEngine;

[CreateAssetMenu(fileName = "P09_Fume", menuName = "Enemy/Actions/Protect/P09_Fume")]
public class P09_Fume : EnemyAction
{
    [SerializeField] private int protectPoint = 8;
    [SerializeField] private int slowPoint = 1;

    public override ActionType Type => ActionType.Protect | ActionType.Debuff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
        BattleManager.Instance.Player.Slow(slowPoint);
    }
}