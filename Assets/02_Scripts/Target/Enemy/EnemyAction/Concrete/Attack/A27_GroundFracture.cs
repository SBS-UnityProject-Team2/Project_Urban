using UnityEngine;

[CreateAssetMenu(fileName = "A27_GroundFracture", menuName = "Enemy/Actions/Attack/A27_GroundFracture")]
public class A27_GroundFracture : EnemyAction
{
    [SerializeField] private int damage = 25;
    [SerializeField] private int count = 1;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Ruin;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}