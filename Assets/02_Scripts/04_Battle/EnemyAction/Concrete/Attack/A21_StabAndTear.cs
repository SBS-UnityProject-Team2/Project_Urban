using UnityEngine;

[CreateAssetMenu(fileName = "A21_StabAndTear", menuName = "Enemy/Actions/Attack/A21_StabAndTear", order = 21)]
public class A21_StabAndTear : AttackAction
{
    [SerializeField] private int damage = 6;
    [SerializeField] private int count = 2;
    [SerializeField] private int bleedPoint = 6;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.None;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Bleed(bleedPoint);
    }
}