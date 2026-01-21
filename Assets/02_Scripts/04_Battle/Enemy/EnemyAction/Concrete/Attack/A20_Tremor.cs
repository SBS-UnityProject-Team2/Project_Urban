using UnityEngine;

[CreateAssetMenu(fileName = "A20_Tremor", menuName = "Enemy/Actions/Attack/A20_Tremor")]
public class A20_Tremor : EnemyAction
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int count = 1;
    [SerializeField] private int exhaustPoint = 1;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Exhaust(exhaustPoint);
    }
}