using UnityEngine;

[CreateAssetMenu(fileName = "P11_Posing", menuName = "Enemy/Actions/Protect/P11_Posing", order = 11)]
public class P11_Posing : EnemyAction
{
    [SerializeField] private int protectPoint = 15;
    [SerializeField] private int elasticVeilPoint = 1;

    public override ActionType Type => ActionType.Protect;
    public override Element Element => Element.None;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Protect(protectPoint);
    }
}