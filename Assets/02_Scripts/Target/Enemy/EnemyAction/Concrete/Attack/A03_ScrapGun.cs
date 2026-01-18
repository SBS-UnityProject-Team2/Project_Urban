using UnityEngine;

[CreateAssetMenu(fileName = "A03_ScrapGun", menuName = "Enemy/Actions/Attack/A03_ScrapGun")]
public class A03_ScrapGun : EnemyAction
{
    [SerializeField] private int damage = 12;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
    }
}