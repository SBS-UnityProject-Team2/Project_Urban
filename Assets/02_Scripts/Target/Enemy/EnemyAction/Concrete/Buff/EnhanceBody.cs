using UnityEngine;

[CreateAssetMenu(fileName = "B1_EnhanceBody", menuName = "Enemy/Actions/Buff/B1_EnhanceBody")]
public class EnhanceBody : EnemyAction
{
    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.Flame;

    public override void Execute(Target target)
    {
        
    }
}