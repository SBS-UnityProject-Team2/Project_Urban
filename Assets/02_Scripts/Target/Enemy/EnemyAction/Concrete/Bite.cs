using UnityEngine;

[CreateAssetMenu(fileName = "Bite", menuName = "Enemy/Actions/Bite")]
public class Bite : EnemyAction
{
    [SerializeField] private int damage = 6;
    [SerializeField] private int bleedCount = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Damage(damage);
        target.ApplyStatusEffect(new Bleed(bleedCount));
    }
}