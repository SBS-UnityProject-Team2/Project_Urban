using UnityEngine;

[CreateAssetMenu(fileName = "P0_Curl", menuName = "Enemy/Actions/Protect/P0_Curl")]
public class Curl : EnemyAction
{
    [SerializeField] private int shield = 7;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.AddProtect(shield);
    }
}