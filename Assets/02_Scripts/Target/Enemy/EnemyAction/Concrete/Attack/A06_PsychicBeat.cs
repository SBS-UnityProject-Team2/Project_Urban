using UnityEngine;

[CreateAssetMenu(fileName = "A06_PsychicBeat", menuName = "Enemy/Actions/Attack/A06_PsychicBeat")]
public class A06_PsychicBeat : EnemyAction
{
    [SerializeField] private int damage = 2;
    [SerializeField] private int count = 5;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Psychic;

    public override void Execute(Enemy enemy, Target target)
    {
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}