using UnityEngine;

[CreateAssetMenu(fileName = "B4_Enrage", menuName = "Enemy/Actions/Buff/B4_Enrage")]
public class Enrage : EnemyAction
{
    [SerializeField] private int count = 3;
    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.ApplyStatusEffect(new Reinforce(count));
    }
}