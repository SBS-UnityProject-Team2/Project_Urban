using UnityEngine;

[CreateAssetMenu(fileName = "P2_ScrapShield", menuName = "Enemy/Actions/Protect/P2_ScrapShield")]
public class ScrapShield : EnemyAction
{
    [SerializeField] private int shield = 15;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Target target)
    {
        target.Protect(shield);
        target.ApplyStatusEffect(new Broken());
    }
}