using UnityEngine;

public class KineticGrasp : Attack
{
    
    private int baseCost = 4;      // 원래의 기본 코스트를 저장

    public override CardName Name => CardName.KineticGrasp;

    protected override void Start()
    {
        base.Start(); // JSON 데이터를 받아옴
        
        // 데이터에서 받아온 초기 코스트를 저장
        baseCost = cost;
    }

    private void Update()
    {
        // 실시간 코스트 계산 로직
        UpdateCurrentCost();
    }

    private void UpdateCurrentCost()
    {
        // 1. 현재 핸드의 카드 개수 가져오기
        int handCount = BattleManager.Instance.Player.CurrentHandCount;

        // 2. 감소량 계산 (2장당 1 감소)
        int reduction = handCount / 2;

        // 3. 현재 코스트 계산
        int newCost = baseCost - reduction;

        // 4. 최소 코스트는 0
        if (newCost < 0) newCost = 0;

        // 5. 실제 cost 변수에 적용
        this.cost = newCost;
    }

    // 카드 사용 후 코스트 원상복구
    private void OnDisable()
    {
        if (baseCost != -1)
        {
            this.cost = baseCost;
        }
    }

    public override int Use(Target target)
    {
        Player player = BattleManager.Instance.Player;        
        
        target.Damage(player, damage); 

        return cost;
    }
}