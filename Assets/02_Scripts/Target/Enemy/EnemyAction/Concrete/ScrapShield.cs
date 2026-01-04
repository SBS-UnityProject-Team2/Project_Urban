using UnityEngine;

[CreateAssetMenu(fileName = "ScrapShield", menuName = "Enemy/Actions/ScrapShield")]
public class ScrapShield : EnemyAction
{
    [SerializeField] private int shield = 15;
    [SerializeField] private int remainingTurn = 2;
    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;


    public override void Execute(Target target)
    {
        target.AddProtect(shield);
        // target.ApplyStatusEffect(new Broken(remainingTurn));
    }
}