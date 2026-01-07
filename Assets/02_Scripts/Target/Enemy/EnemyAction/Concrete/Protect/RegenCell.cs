using UnityEngine;

[CreateAssetMenu(fileName = "P4_RegenCell", menuName = "Enemy/Actions/Protect/P4_RegenCell")]
public class RegenCell : EnemyAction
{
    [SerializeField] private int shield = 7;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Protect(shield);
        target.Regeneration(2);
    }
}