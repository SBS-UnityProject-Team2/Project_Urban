using UnityEngine;

[CreateAssetMenu(fileName = "A07_RepulsiveMatrix", menuName = "Enemy/Actions/Attack/A07_RepulsiveMatrix", order = 7)]
public class A07_RepulsiveMatrix : AttackAction
{
    [SerializeField] private int damage = 9;
    [SerializeField] private int count = 1;
    [SerializeField] private int protect = 9;
    public override ActionType Type => ActionType.Attack | ActionType.Protect;
    public override Element Element => Element.Psychic;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {
        target.Damage(enemy, damage, Element);
        target.Protect(protect);   
    }
}