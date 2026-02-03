using UnityEngine;

[CreateAssetMenu(fileName = "A28_Gravity", menuName = "Enemy/Actions/Attack/A28_Gravity", order = 28)]
public class A28_Gravity : AttackAction
{
    [SerializeField] private int damage = 6;
    [SerializeField] private int count = 1;
    [SerializeField] private int weakenPoint = 4;
    [SerializeField] private int slowPoint = 2;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Psychic;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Weaken(weakenPoint);
        target.Slow(slowPoint);
    }
}