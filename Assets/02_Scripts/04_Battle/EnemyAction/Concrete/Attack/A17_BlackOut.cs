using UnityEngine;

[CreateAssetMenu(fileName = "A17_BlackOut", menuName = "Enemy/Actions/Attack/A17_BlackOut", order = 17)]
public class A17_BlackOut : AttackAction
{
    [SerializeField] private int damage = 3;
    [SerializeField] private int count = 3;
    [SerializeField] private int dizzyPoint = 1;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Psychic;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Dizzy(dizzyPoint);
    }
}