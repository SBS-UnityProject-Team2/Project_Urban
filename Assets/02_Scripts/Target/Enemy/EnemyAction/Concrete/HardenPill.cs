using UnityEngine;

[CreateAssetMenu(fileName = "HardenPill", menuName = "Enemy/Actions/HardenPill")]
public class HardenPill : EnemyAction
{
    [SerializeField] private int shield = 8;
    [SerializeField] private int remainingTurn = 3;
    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.AddProtect(shield);
        // target.AddConditionStatus(new Armor(remainingTurn));
    }
}