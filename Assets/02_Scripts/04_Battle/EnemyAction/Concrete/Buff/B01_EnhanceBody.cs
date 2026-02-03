using UnityEngine;

[CreateAssetMenu(fileName = "B01_EnhanceBody", menuName = "Enemy/Actions/Buff/B01_EnhanceBody", order = 1
)]
public class B01_EnhanceBody : EnemyAction
{
    [SerializeField] private int armorPoint = 2;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.Ruin;

    public override void Execute(Enemy enemy, Target target)
    {
        enemy.Armor(armorPoint);
    }
}