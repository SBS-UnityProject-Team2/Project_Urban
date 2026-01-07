using UnityEngine;

[CreateAssetMenu(fileName = "P1_Smoke", menuName = "Enemy/Actions/Protect/P1_Smoke")]
public class Smoke : EnemyAction
{
    [SerializeField] private int shield = 10;
    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Protect(shield);
    }
}