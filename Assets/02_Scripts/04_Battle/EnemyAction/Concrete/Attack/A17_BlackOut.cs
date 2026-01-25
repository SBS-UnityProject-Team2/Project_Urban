using UnityEngine;

[CreateAssetMenu(fileName = "A17_BlackOut", menuName = "Enemy/Actions/Attack/A17_BlackOut")]
public class A17_BlackOut : EnemyAction
{
    [SerializeField] private int damage = 3;
    [SerializeField] private int count = 3;
    [SerializeField] private int dizzyPoint = 1;

    public override ActionType Type => ActionType.Attack | ActionType.Debuff;
    public override Element Element => Element.Psychic;

    public override void Execute(Enemy enemy, Target target)
    {   
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);

        target.Dizzy(dizzyPoint);
    }
}