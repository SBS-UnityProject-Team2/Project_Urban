using UnityEngine;

[CreateAssetMenu(fileName = "D0_Roar", menuName = "Enemy/Actions/Debuff/D0_Roar")]
public class Roar : EnemyAction
{
    public override ActionType Type => ActionType.Debuff;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        
    }
}