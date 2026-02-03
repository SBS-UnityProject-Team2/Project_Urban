using UnityEngine;

[CreateAssetMenu(fileName = "A09_CorrosiveShock", menuName = "Enemy/Actions/Attack/A09_CorrosiveShock", order = 9)]
public class A09_CorrosiveShock : AttackAction
{
    [SerializeField] private int damage = 7;
    [SerializeField] private int count = 2;
    [SerializeField] private int brokenPoint = 2;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Bio;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
            
        target.Broken(brokenPoint);
    }
}