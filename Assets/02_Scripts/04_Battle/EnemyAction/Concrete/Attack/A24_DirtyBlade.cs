using UnityEngine;

[CreateAssetMenu(fileName = "A24_DirtyBlade", menuName = "Enemy/Actions/Attack/A24_DirtyBlade", order = 24)]
public class A24_DirtyBlade : AttackAction
{
    [SerializeField] private int damage = 12;
    [SerializeField] private int count = 1;
    [SerializeField] private int poisonedPoint = 1;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Bio;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Poisoned(poisonedPoint);
    }
}