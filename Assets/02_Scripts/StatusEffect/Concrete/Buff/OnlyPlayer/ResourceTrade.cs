
using System.Collections.Generic; 

public class ResourceTrade : PlayerStatusEffect
{
    public ResourceTrade(Player player) : base(player)
    {
        player.OnTurnStart.AddListener(HandleTurnStart);
    }

    public override int StatusNumber => 0;
    public override StatusEffectName Name => StatusEffectName.ResourceTrade;

    public void Active()
    {
        SetActive(true);
    }

    private void HandleTurnStart()
    {
        if (!IsActive) return;

        //  만약 버리기에 성공하면(콜백), 회복하고 코스트 증가 실행
        player.DiscardCard(1, (discardedCards) => 
        {
            // 플레이어가 UI에서 카드를 고르고 확인을 눌렀을 때 실행
            player.Heal(1);
            player.Cost.Increase();
        });
    }
}