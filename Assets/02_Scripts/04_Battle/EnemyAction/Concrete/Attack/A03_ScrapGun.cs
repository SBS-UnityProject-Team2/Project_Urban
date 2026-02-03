using UnityEngine;

[CreateAssetMenu(fileName = "A03_ScrapGun", menuName = "Enemy/Actions/Attack/A03_ScrapGun", order = 3)]
public class A03_ScrapGun : AttackAction
{
    [SerializeField] private int damage = 12;
    [SerializeField] private int count = 1;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
    }
}