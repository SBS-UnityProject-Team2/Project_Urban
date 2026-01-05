using UnityEngine;

[CreateAssetMenu(fileName = "D1_ArmorBreak", menuName = "Enemy/Actions/Debuff/D1_ArmorBreak")]
public class ArmorBreak : EnemyAction
{
    public override ActionType Type => ActionType.Debuff;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.ApplyStatusEffect(new Broken());
        target.ApplyStatusEffect(new Armor());
    }
}