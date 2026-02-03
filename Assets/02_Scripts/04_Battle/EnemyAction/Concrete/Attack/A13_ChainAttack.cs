using UnityEngine;

[CreateAssetMenu(fileName = "A13_ChainAttack", menuName = "Enemy/Actions/Attack/A13_ChainAttack", order = 13)]
public class A13_ChainAttack : AttackAction
{
    [SerializeField] private int damage = 14;
    [SerializeField] private int count = 1;
    [SerializeField] private int brokenPoint = 3;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Ruin;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Broken(brokenPoint);
    }
}