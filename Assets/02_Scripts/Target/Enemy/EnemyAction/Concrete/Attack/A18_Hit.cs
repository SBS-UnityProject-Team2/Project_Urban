using UnityEngine;

[CreateAssetMenu(fileName = "A18_Hit", menuName = "Enemy/Actions/Attack/A18_Hit")]
public class A18_Hit : EnemyAction
{
    [SerializeField] private int damage = 4;
    [SerializeField] private int count = 1;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}