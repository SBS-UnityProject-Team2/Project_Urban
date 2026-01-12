using UnityEngine;

[CreateAssetMenu(fileName = "B0_Sharpening", menuName = "Enemy/Actions/Buff/B0_Sharpening")]
public class Sharpening : EnemyAction
{
    [SerializeField] private int damage = 9;
    [SerializeField] private int count = 1;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        //target.IncreaseAdditionalDamage(damage);
        //target.IncreaseAdditionalDamageCount(count);
    }
}