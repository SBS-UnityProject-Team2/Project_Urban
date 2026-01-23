using UnityEngine;

[CreateAssetMenu(fileName = "A12_SandSpray", menuName = "Enemy/Actions/Attack/A12_SandSpray")]
public class A12_SandSpray : EnemyAction
{
    [SerializeField] private int damage = 8;
    [SerializeField] private int count = 1;
    [SerializeField] private int weakenPoint = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Weaken(weakenPoint);
    }
}