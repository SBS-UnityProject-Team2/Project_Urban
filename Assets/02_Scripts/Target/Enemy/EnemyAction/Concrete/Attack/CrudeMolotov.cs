using UnityEngine;

[CreateAssetMenu(fileName = "A4_CrudeMolotov", menuName = "Enemy/Actions/Attack/A4_CrudeMolotov")]
public class CrudeMolotov : EnemyAction
{
    [SerializeField] private int damage = 4;
    [SerializeField] private int count = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Flame;

    public override void Execute(Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(damage);
            
        target.ApplyStatusEffect(new Burn(count));
    }
}