using UnityEngine;

[CreateAssetMenu(fileName = "P03_HardenPill", menuName = "Enemy/Actions/Protect/P03_HardenPill")]
public class P03_HardenPill : EnemyAction
{
    [SerializeField] private int protectPoint = 8;
    [SerializeField] private int armorPoint = 2;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
        target.Armor(armorPoint);
    }
}