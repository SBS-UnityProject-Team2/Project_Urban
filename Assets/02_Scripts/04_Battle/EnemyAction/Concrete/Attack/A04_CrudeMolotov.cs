using UnityEngine;

[CreateAssetMenu(fileName = "A04_CrudeMolotov", menuName = "Enemy/Actions/Attack/A04_CrudeMolotov")]
public class A04_CrudeMolotov : EnemyAction
{
    [SerializeField] private int damage = 4;
    [SerializeField] private int count = 2;
    [SerializeField] private int burnPoint = 2;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Ruin;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
            
        target.Burn(burnPoint);
    }
}