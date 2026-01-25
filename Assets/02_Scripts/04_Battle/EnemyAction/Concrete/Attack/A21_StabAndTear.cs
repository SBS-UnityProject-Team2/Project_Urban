using UnityEngine;

[CreateAssetMenu(fileName = "A21_StabAndTear", menuName = "Enemy/Actions/Attack/A21_StabAndTear")]
public class A21_StabAndTear : EnemyAction
{
    [SerializeField] private int damage = 6;
    [SerializeField] private int count = 2;
    [SerializeField] private int bleedPoint = 6;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Bleed(bleedPoint);
    }
}