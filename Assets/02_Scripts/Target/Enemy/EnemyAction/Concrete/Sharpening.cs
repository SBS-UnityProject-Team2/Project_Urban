using UnityEngine;

[CreateAssetMenu(fileName = "Sharpening", menuName = "Enemy/Actions/Sharpening")]
public class Sharpening : EnemyAction
{
    [SerializeField] private int damage = 9;
    [SerializeField] private int remainingTurn = int.MaxValue;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        //target.AddConditionStatus(new ExtraDamage(remainingTurn, damage));
    }
}