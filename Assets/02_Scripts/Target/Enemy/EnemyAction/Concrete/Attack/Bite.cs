using UnityEngine;

[CreateAssetMenu(fileName = "A1_Bite", menuName = "Enemy/Actions/Attack/A1_Bite")]
public class Bite : EnemyAction
{
    [SerializeField] private int damage = 6;
    [SerializeField] private int bleedCount = 5;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Damage(damage);
        target.ApplyStatusEffect(new Bleed(bleedCount));
    }
}