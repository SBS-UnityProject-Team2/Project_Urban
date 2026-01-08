using UnityEngine;

[CreateAssetMenu(fileName = "A6_PsychicBeat", menuName = "Enemy/Actions/Attack/A6_PsychicBeat")]
public class PsychicBeat : EnemyAction
{
    [SerializeField] private int count = 5;
    [SerializeField] private int damage = 2;
    public override ActionType Type => ActionType.Attack;
    public override Element Element => Element.Psychic;

    public override void Execute(Target target)
    {
        // for (int i = 0; i < count; i++)
            // target.Damage(damage);
    }
}