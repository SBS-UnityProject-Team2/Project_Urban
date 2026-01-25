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

        // BattleManager.Instance.Player.Hand.OnUpdateHand.AddListener(UpdateCurrentCost);
        UpdateCurrentCost();
    }

    private void OnDisable()
    {
       // BattleManager.Instance.Player.Hand.OnUpdateHand.RemoveListener(UpdateCurrentCost);
    }

    private void UpdateCurrentCost()
    {
        // 1. 현재 핸드의 카드 개수 가져오기
        int handCount = BattleManager.Instance.Player.CurrentHandCount;

        // 2. 감소량 계산 (2장당 1 감소)
        int reduction = handCount / 2;

        // 3. 현재 코스트 계산
        int newCost = initCost - reduction;

        // 4. 최소 코스트는 0
        if (newCost < 0) newCost = 0;

        SetCost(newCost);
    }

    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage, Element.Psychic); 

        return curCost;
    }
}