using UnityEngine;

[CreateAssetMenu(fileName = "A11_Steal", menuName = "Enemy/Actions/Attack/A11_Steal", order = 11)]
public class A11_Steal : AttackAction
{
    [SerializeField] private int damage = 10;
    [SerializeField] private int count = 1;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        // 골드 훔치기
    }
}