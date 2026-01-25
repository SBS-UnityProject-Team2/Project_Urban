using UnityEngine;

[CreateAssetMenu(fileName = "A08_Bash", menuName = "Enemy/Actions/Attack/A08_Bash")]
public class A08_Bash : EnemyAction
{
    [SerializeField] private int damage = 8;
    [SerializeField] private int count = 1;
    [SerializeField] private int weakenPoint = 2;
    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Bio;

    public override void Execute(Enemy enemy, Target target)
    {
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Weaken(weakenPoint);
    }
}