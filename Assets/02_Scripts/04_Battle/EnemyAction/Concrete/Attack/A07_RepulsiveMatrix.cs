using UnityEngine;

[CreateAssetMenu(fileName = "A07_RepulsiveMatrix", menuName = "Enemy/Actions/Attack/A07_RepulsiveMatrix")]
public class A07_RepulsiveMatrix : EnemyAction
{
    [SerializeField] private int damage = 9;
    [SerializeField] private int protect = 9;
    public override ActionType Type => ActionType.Attack | ActionType.Protect;
    public override Element Element => Element.Psychic;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
        target.Protect(protect);   
    }
}