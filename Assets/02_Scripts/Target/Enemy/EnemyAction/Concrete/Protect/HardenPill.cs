using UnityEngine;

[CreateAssetMenu(fileName = "P3_HardenPill", menuName = "Enemy/Actions/Protect/P3_HardenPill")]
public class HardenPill : EnemyAction
{
    [SerializeField] private int shield = 8;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Protect(shield);
        target.Armor(2);
    }
}