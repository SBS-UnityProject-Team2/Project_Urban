using UnityEngine;

[CreateAssetMenu(fileName = "P04_RegenCell", menuName = "Enemy/Actions/Protect/P04_RegenCell", order = 4)]
public class P04_RegenCell : EnemyAction
{
    [SerializeField] private int protectPoint = 7;
    [SerializeField] private int regenerationPoint = 2;

    public override ActionType Type => ActionType.Protect | ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
        target.Regeneration(regenerationPoint);
    }
}