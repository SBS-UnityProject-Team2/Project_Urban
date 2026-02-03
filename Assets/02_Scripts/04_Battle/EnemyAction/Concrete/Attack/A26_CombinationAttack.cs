using UnityEngine;

[CreateAssetMenu(fileName = "A26_CombinationAttack", menuName = "Enemy/Actions/Attack/A26_CombinationAttack", order = 26)]
public class A26_CombinationAttack : AttackAction
{
    [SerializeField] private int damage = 5;
    [SerializeField] private int count = 4;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Ruin;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}