using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Text;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class EventButtonUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text rewardText;
    
    // Component
    private Button button;

    // Data
    private EventChoice eventChoice;
    private EventReward eventReward;

    private int earnHp;
    private int earnCoin;

    public void Init()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }

    public void SetChoice(int choiceCode, UnityAction<EventRewardData> handleClick)
    {
        eventChoice = EventManager.Instance.GetEventChoice(choiceCode);
        eventReward = EventManager.Instance.GetEventReward(eventChoice.rewardCode);

        SetHpDelta(eventReward.hpPresent, eventReward.hpMax);
        SetCoinDelta(eventReward.gold);

        titleText.text = eventChoice.choiceName;
        rewardText.text = BuildResultString();

        button.enabled = CheckCondition(eventChoice.choiceCondition);
        button.onClick.AddListener(() =>
        {
            PlayerManager.Instance.Health.IncreaseHp(earnHp);
            handleClick?.Invoke(GetRewardData());
        });
    }

    private EventRewardData GetRewardData()
    {
        return new()
        {
            earnHp = earnHp,
            earnCoin = earnCoin,
            randomCard = eventReward.randomCard,
            removeCard = eventReward.remove,
            cards = eventReward.selectCards,
            scriptCode = eventChoice.scriptCode
        };
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
            stringBuilder.Append($"[{GetCardPoolName(eventReward.randomCard)}] 속성 카드 1개 랜덤 획득\n");

        if (eventReward.selectCards.Length != 0)
            stringBuilder.Append(BuildRangeCardString(eventReward.selectCards));

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
            1 => "물리",
            2 => "파괴",
            3 => "사이킥",
            4 => "생체",
            5 => "전체",
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

    private string BuildRangeCardString(CardName [] selectCards)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append("카드 ");

        for (int i = 0; i < selectCards.Length; i++)
        {
            CardDataEntry data = CardManager.Instance.GetCardData(selectCards[i]);
            stringBuilder.Append($"[{data.koreanName}]");

            if (i < selectCards.Length - 1)
                stringBuilder.Append(", ");
        }

        stringBuilder.Append(" 중 1개 선택 획득\n");

        return stringBuilder.ToString();
    }

    private bool CheckCondition(int condition)
    {
        if (condition == 0) 
            return true;

        Element element = (Element)(condition * 1000);

        return DeckManager.Instance.CardList.Any(card => card.Element == element);
    }
}
