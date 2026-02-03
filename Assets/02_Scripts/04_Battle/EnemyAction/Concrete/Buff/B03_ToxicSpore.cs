using UnityEngine;

[CreateAssetMenu(fileName = "B03_ToxicSpore", menuName = "Enemy/Actions/Buff/B03_ToxicSpore", order = 3
)]
public class B03_ToxicSpore : EnemyAction
{
    [SerializeField] private int damage = 3;
    [SerializeField] private int buffTurn = 4;

    public override ActionType Type => ActionType.Buff;
    public override Element Element => Element.Bio;

    private int remainingTurn;

    public override void Execute(Enemy enemy, Target target)
    {
        remainingTurn = buffTurn;

        enemy.OnAttack.AddListener(HandleAttack);
    }

    private void HandleAttack(Target attacker, Target target)
    {
        target.DebuffDamage(damage);
        remainingTurn--;

        if (remainingTurn == 0)
            attacker.OnAttack.RemoveListener(HandleAttack);
    }
}