using System.Collections;

class Strike : Attack
{
    public override CardName Name => CardName.Strike;

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Damage(user, damage);
    }
}