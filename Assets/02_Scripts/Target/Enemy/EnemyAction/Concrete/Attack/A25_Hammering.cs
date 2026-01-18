using UnityEngine;

[CreateAssetMenu(fileName = "A25_Hammering", menuName = "Enemy/Actions/Attack/A25_Hammering")]
public class A25_Hammering : EnemyAction
{
    [SerializeField] private int damage = 16;
    [SerializeField] private int count = 1;
    [SerializeField] private int brokenPoint = 1;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Ruin;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Broken(brokenPoint);
    }
}