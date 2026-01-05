using UnityEngine;

[CreateAssetMenu(fileName = "A3_ScrapGun", menuName = "Enemy/Actions/Attack/A3_ScrapGun")]
public class ScrapGun : EnemyAction
{
    [SerializeField] private int damage = 12;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Damage(damage);
    }
}