using UnityEngine;

[CreateAssetMenu(fileName = "P5_Barrier", menuName = "Enemy/Actions/Protect/P5_Barrier")]
public class Barrier : EnemyAction
{
    [SerializeField] private int shield = 12;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Protect(shield);
    }
}