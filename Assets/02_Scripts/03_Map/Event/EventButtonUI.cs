using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text;

[RequireComponent(typeof(Button))]
public class EventButtonUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text choiceTitleText;
    [SerializeField] private TMP_Text hoverRewardText;
    [SerializeField] private Image buttonImage; // 배경 이미지
    
    // Component
    private Button button;

    // Data
    private EventChoice eventChoice;
    private EventReward eventReward;
    private EventResult eventResult;

    private int earnHp;
    private int earnCoin;

    public void Init(int choiceCode)
    {
        button = GetComponent<Button>();
        eventChoice = EventManager.Instance.GetEventChoice(choiceCode);
        eventReward = EventManager.Instance.GetEventReward(eventChoice.resultCode);
        eventResult = EventManager.Instance.GetEventResult(eventChoice.scriptCode);

        SetHpDelta(eventReward.hpPresent, eventReward.hpMax);
        SetCoinDelta(eventReward.gold);

        choiceTitleText.text = eventChoice.choiceName;
        hoverRewardText.text = BuildResultString();

        button.enabled = CheckCondition((ConditionType)eventChoice.choiceCondition);
    }

    private void SetHpDelta(float presentHpRatio, float maxHpRatio)
    {
        int curPlayerHp = PlayerManager.Instance.Health.CurrentHp;
        int maxPlayerHp = PlayerManager.Instance.Health.MaxHp;

        earnHp = (int)(curPlayerHp * presentHpRatio + maxPlayerHp * maxHpRatio);
    }

    private void SetCoinDelta(int coin)
    {
        if (coin == 0)
        {
            earnCoin = 0;
            
            return;
        }

        float coinDelta = coin * EventManager.Instance.CoinRatio;

        int minCoin = (int)(coin - coinDelta);
        int maxCoin = (int)(coin + coinDelta);

        earnCoin = Random.Range(minCoin, maxCoin + 1);
    }

    private string BuildResultString()
    {
        StringBuilder stringBuilder = new();
        
        if (earnHp != 0)
            stringBuilder.Append($"체력 [{Mathf.Abs(earnHp)}] {(earnHp > 0 ? "회복" : "피해")}\n");

        if (earnCoin != 0)
            stringBuilder.Append($"금액 [{earnCoin}] {(earnCoin > 0 ? "획득" : "소실")}\n");

        if (eventReward.randomCard != 0)
            stringBuilder.Append($"[{GetCardPoolName(eventReward.randomCard)}] 속성 카드 1개 획득\n");

        if (eventReward.rangeCard != 0)
            stringBuilder.Append(BuildRangeCardString(eventReward.rangeCard));

        if (eventReward.remove != 0)
            stringBuilder.Append($"[{GetRemoveElementName(eventReward.remove)}] 속성 카드 1개 랜덤 제거");

        if (stringBuilder.Length == 0)
            stringBuilder.Append("없음");

        return stringBuilder.ToString();
    }

    private string GetCardPoolName (int randomCardPoolCode)
    {
        return randomCardPoolCode switch
        {
            1 => "랜덤",
            2 => "물리",
            3 => "파괴",
            4 => "사이킥",
            5 => "생체",
            _ => "",  
        };   
    }

    private string GetRemoveElementName(int removeCode)
    {
        return removeCode switch
        {
            1 => "물리",
            2 => "파괴",
            3 => "사이킥",
            4 => "생체",
            5 => "전체",
            _ => "",  
        };   
    }

    private string BuildRangeCardString(int rangeCardPoolCode)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append("카드 ");

        RangeCardPool rangeCardPool = EventManager.Instance.GetRangeCardPool(rangeCardPoolCode);

        CardDataEntry data = CardManager.Instance.GetCardData((CardName)rangeCardPool.card1);
        stringBuilder.Append($"[{data.koreanName}]");

        if (rangeCardPool.card2 != 0)
        {
            data = CardManager.Instance.GetCardData((CardName)rangeCardPool.card2);
            stringBuilder.Append($", [{data.koreanName}]");
        }

        if (rangeCardPool.card3 != 0)
        {
            data = CardManager.Instance.GetCardData((CardName)rangeCardPool.card3);
            stringBuilder.Append($", [{data.koreanName}]");
        }

        stringBuilder.Append(" 중 1개 선택 획득\n");

        return stringBuilder.ToString();
    }

    public static bool CheckCondition(ConditionType condition)
    {
        // 조건이 없으면 항상 활성화
        if (condition == ConditionType.None) 
            return true;

        // 조건 Enum → Element 변환
        Element requiredElement = condition switch
        {
            ConditionType.RequireNone => Element.None,
            ConditionType.RequireRuin => Element.Ruin,
            ConditionType.RequirePsychic => Element.Psychic,
            ConditionType.RequireBio => Element.Bio,
            _ => Element.None
        };

        // 덱에 필요한 속성 카드가 있는지 확인
        return DeckManager.Instance.CardList.Any(card => card.Element == requiredElement);
    }


}

public enum ConditionType
{
    None,
    RequireNone,
    RequireRuin,
    RequirePsychic,
    RequireBio
}