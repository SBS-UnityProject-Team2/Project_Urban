using UnityEngine;

[CreateAssetMenu(fileName = "A25_Hammering", menuName = "Enemy/Actions/Attack/A25_Hammering", order = 25)]
public class A25_Hammering : AttackAction
{
    [SerializeField] private int damage = 16;
    [SerializeField] private int count = 1;
    [SerializeField] private int brokenPoint = 1;

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