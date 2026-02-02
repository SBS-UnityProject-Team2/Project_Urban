using System.Collections;

public class KineticGrasp : Attack
{
    public override CardName Name => CardName.KineticGrasp;

    protected override void Start()
    {
        base.Start();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        UpdateCurrentCost();
    }

    private void OnDisable()
    {
    }

    private void UpdateCurrentCost()
    {
        int handCount = BattleManager.Instance.Player.CurrentHandCount;
        int reduction = handCount / 2;
        int newCost = initCost - reduction;
        if (newCost < 0) newCost = 0;
        SetCost(newCost);
    }

    protected override IEnumerator UseRoutine(Player user, Target target)
    {
        yield return PlayEffect(target);
        target.Damage(user, damage, Element.Psychic);
    }
}