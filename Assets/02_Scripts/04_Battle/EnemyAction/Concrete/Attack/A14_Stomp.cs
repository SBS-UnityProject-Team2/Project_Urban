using UnityEngine;

[CreateAssetMenu(fileName = "A14_Stomp", menuName = "Enemy/Actions/Attack/A14_Stomp", order = 14)]
public class A14_Stomp : AttackAction
{
    [SerializeField] private int damage = 9;
    [SerializeField] private int count = 2;
    [SerializeField] private int brokenPoint = 3;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.None;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Broken(brokenPoint);
    }
}