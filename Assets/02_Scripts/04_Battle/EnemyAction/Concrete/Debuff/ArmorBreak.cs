using UnityEngine;

[CreateAssetMenu(fileName = "D1_ArmorBreak", menuName = "Enemy/Actions/Debuff/D1_ArmorBreak", order = 1)]
public class ArmorBreak : EnemyAction
{
    [SerializeField] private int brokenPoint;
    [SerializeField] private int armorPoint;

    public override ActionType Type => ActionType.Debuff | ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Broken(brokenPoint);
        target.Armor(armorPoint);
    }
}