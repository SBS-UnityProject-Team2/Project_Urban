using UnityEngine;

[CreateAssetMenu(fileName = "A11_Steal", menuName = "Enemy/Actions/Attack/A11_Steal")]
public class A11_Steal : EnemyAction
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int count = 1;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        // 골드 훔치기
    }
}