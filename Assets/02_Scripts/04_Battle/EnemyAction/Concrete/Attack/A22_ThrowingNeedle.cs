using UnityEngine;

[CreateAssetMenu(fileName = "A22_ThrowingNeedle", menuName = "Enemy/Actions/Attack/A22_ThrowingNeedle")]
public class A22_ThrowingNeedle : EnemyAction
{
    [SerializeField] private int damage = 8;
    [SerializeField] private int count = 1;
    [SerializeField] private int scarredPoint = 2;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Scarred(scarredPoint);
    }
}