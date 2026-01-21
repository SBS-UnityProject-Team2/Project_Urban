using UnityEngine;

[CreateAssetMenu(fileName = "A29_Echoes", menuName = "Enemy/Actions/Attack/A29_Echoes")]
public class A29_Echoes : EnemyAction
{
    [SerializeField] private int damage = 8;
    [SerializeField] private int count = 1;
    [SerializeField] private int exhaustPoint = 2;
    [SerializeField] private int dizzyPoint = 2;

    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Psychic;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Exhaust(exhaustPoint);
        target.Dizzy(dizzyPoint);
    }
}