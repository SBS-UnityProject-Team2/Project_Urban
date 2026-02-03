using UnityEngine;

[CreateAssetMenu(fileName = "A12_SandSpray", menuName = "Enemy/Actions/Attack/A12_SandSpray", order = 12)]
public class A12_SandSpray : AttackAction
{
    [SerializeField] private int damage = 8;
    [SerializeField] private int count = 1;
    [SerializeField] private int weakenPoint = 2;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.None;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Weaken(weakenPoint);
    }
}