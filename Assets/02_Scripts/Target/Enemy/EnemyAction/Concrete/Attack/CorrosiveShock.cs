using UnityEngine;

[CreateAssetMenu(fileName = "A9_CorrosiveShock", menuName = "Enemy/Actions/Attack/A9_CorrosiveShock")]
public class CorrosiveShock : EnemyAction
{
    [SerializeField] private int damage = 7;
    [SerializeField] private int count = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Grass;

    public override void Execute(Target target)
    {   
        for (int i = 0; i < count; i++)
            // target.Damage(damage);
            
        target.ApplyStatusEffect(new Broken());
    }
}