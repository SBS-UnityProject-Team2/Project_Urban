using UnityEngine;

[CreateAssetMenu(fileName = "A15_Swing", menuName = "Enemy/Actions/Attack/A15_Swing")]
public class A15_Swing : EnemyAction
{
    [SerializeField] private int damage = 15;
    [SerializeField] private int count = 1;
    [SerializeField] private int brokenPoint = 3;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Ruin;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Broken(brokenPoint);
    }
}