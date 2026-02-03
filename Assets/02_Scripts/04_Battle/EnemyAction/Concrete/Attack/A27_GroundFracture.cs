using UnityEngine;

[CreateAssetMenu(fileName = "A27_GroundFracture", menuName = "Enemy/Actions/Attack/A27_GroundFracture", order = 27)]
public class A27_GroundFracture : AttackAction
{
    [SerializeField] private int damage = 25;
    [SerializeField] private int count = 1;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Ruin;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}