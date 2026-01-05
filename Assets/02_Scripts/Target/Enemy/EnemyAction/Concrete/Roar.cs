using UnityEngine;

[CreateAssetMenu(fileName = "Roar", menuName = "Enemy/Actions/Roar")]
public class Roar : EnemyAction
{
    [SerializeField] private int remainingTurn = 2;
    public override ActionType Type => ActionType.Debuff;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        //target.AddConditionStatus(new Weaken(remainingTurn));
    }
}