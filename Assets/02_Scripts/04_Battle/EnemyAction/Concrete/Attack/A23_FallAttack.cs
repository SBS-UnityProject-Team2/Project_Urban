using UnityEngine;

[CreateAssetMenu(fileName = "A23_FallAttack", menuName = "Enemy/Actions/Attack/A23_FallAttack", order = 23)]
public class A23_FallAttack : AttackAction
{
    [SerializeField] private int damage = 16;
    [SerializeField] private int count = 1;
    [SerializeField] private int scarredPoint = 2;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Bio;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Scarred(scarredPoint);
    }
}