using UnityEngine;

[CreateAssetMenu(fileName = "B3_ToxicSpore", menuName = "Enemy/Actions/Buff/B3_ToxicSpore")]
public class ToxicSpore : EnemyAction
{
    [SerializeField] private int damage = 3;
    [SerializeField] private int count = 4;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.Bio;

    public override void Execute(Target target)
    {
        target.IncreaseAdditionalDamage(damage);
        target.IncreaseAdditionalDamageCount(count);
    }
}