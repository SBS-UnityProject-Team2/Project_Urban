using UnityEngine;

[CreateAssetMenu(fileName = "A06_PsychicBeat", menuName = "Enemy/Actions/Attack/A06_PsychicBeat", order = 6)]
public class A06_PsychicBeat : AttackAction
{
    [SerializeField] private int damage = 2;
    [SerializeField] private int count = 5;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Psychic;
    public override int Damage => damage;
    public override int Count => count;

    public override void Execute(Enemy enemy, Target target)
    {
        for (int i = 0; i < count; i++)
            target.Damage(enemy, damage, Element);
    }
}