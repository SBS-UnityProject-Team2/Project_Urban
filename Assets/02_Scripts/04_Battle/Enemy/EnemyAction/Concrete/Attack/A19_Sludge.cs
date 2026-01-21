using UnityEngine;

[CreateAssetMenu(fileName = "A19_Sludge", menuName = "Enemy/Actions/Attack/A19_Sludge")]
public class A19_Sludge : EnemyAction
{
    [SerializeField] private int damage = 6;
    [SerializeField] private int count = 1;
    [SerializeField] private int weakenPoint = 3;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Bio;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Weaken(weakenPoint);
    }
}