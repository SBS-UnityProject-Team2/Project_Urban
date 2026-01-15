public class KineticGrasp : Attack
{
    public override CardName Name => CardName.KineticGrasp;

    protected override void Start()
    {
        base.Start();
        BattleManager.Instance.Player.OnUseCard.AddListener(UpdateCurrentCost);
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

        // 5. 실제 cost 변수에 적용
        curCost = newCost;
    }

    // 카드 사용 후 코스트 원상복구
    private void OnDisable()
    {
        curCost = initCost;
    }

    public override int Use(Player player, Target target)
    {
        target.Damage(player, damage, Element.Psychic); 

        return curCost;
    }
}