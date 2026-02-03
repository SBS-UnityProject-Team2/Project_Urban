using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventRewardUI : MonoBehaviour
{
    [Header("Button Reference")]
    [SerializeField] private Button coinButton;
    [SerializeField] private Button selectCardButton;
    [SerializeField] private Button randomCardButton;
    [SerializeField] private Button removeCardButton;
    [SerializeField] private Button confirmButton;

    [Header("UI Reference")]
    [SerializeField] private SelectCardUI selectCardUI;
    [SerializeField] private EventRewardCardUI rewardCardUI;

    // EventResult에 데이터 전달용
    private readonly EventResultData resultData = new();

    private TMP_Text coinText;
    private TMP_Text randomCardText;
    private TMP_Text removeCardText;

    private TMP_Text CoinText => coinText = coinText != null ? coinText : coinButton.GetComponentInChildren<TMP_Text>();
    private TMP_Text RandomCardText => randomCardText = randomCardText != null ? randomCardText : randomCardButton.GetComponentInChildren<TMP_Text>();
    private TMP_Text RemoveCardText => removeCardText = removeCardText != null ? removeCardText : removeCardButton.GetComponentInChildren<TMP_Text>();

    private readonly StringBuilder builder = new();

    public void Init()
    {
        resultData.Reset();
        rewardCardUI.gameObject.SetActive(false);

        coinButton.gameObject.SetActive(false);
        coinButton.onClick.RemoveAllListeners();

        selectCardButton.gameObject.SetActive(false);
        selectCardButton.onClick.RemoveAllListeners();

        randomCardButton.gameObject.SetActive(false);
        randomCardButton.onClick.RemoveAllListeners();

        removeCardButton.gameObject.SetActive(false);
        removeCardButton.onClick.RemoveAllListeners();

        confirmButton.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }

    public void SetReward(EventRewardData eventRewardData, UnityAction<int, string> handleConfirm)
    {
        resultData.earnHp = eventRewardData.earnHp;

        if (eventRewardData.earnCoin != 0)
            SetCoinButton(eventRewardData.earnCoin);

        if (eventRewardData.cards.Length != 0)
            SetSelectCardButton(eventRewardData.cards);

        if (eventRewardData.randomCard != 0)
            SetRandomCardButton(eventRewardData.randomCard);

        if (eventRewardData.removeCard != 0)
            SetRemoveCardButton(eventRewardData.removeCard);

        confirmButton.onClick.AddListener(() => handleConfirm(eventRewardData.scriptCode, BuildResultString()));
    }

    // 코인 텍스트 변경, 클릭시 코인을 얻도록 세팅
    private void SetCoinButton(int coin)
    {
        CoinText.text = $"{coin} Coin";
        coinButton.onClick.AddListener(() =>
        {
            PlayerManager.Instance.Coin.Increase(coin);
            resultData.earnCoin = coin;

            coinButton.gameObject.SetActive(false);
        });

        coinButton.gameObject.SetActive(true);
    }

    // 클릭 시, 카드 선택 팝업이 열리도록 세팅
    // 각 카드 선택 시, 버튼 삭제하고 그냥 닫으면 삭제 안함
    private void SetSelectCardButton(CardName[] cardNames)
    {
        selectCardButton.onClick.AddListener(() =>
        {
            ToggleSelectCard(true);

            selectCardUI.Init();
            selectCardUI.SetSelectCards(cardNames, HandleSelectCard, () => ToggleSelectCard(false));
        });

        selectCardButton.gameObject.SetActive(true);
    }

    private void HandleSelectCard(CardName cardName)
    {
        resultData.selectedCard = cardName;
        selectCardButton.gameObject.SetActive(false);

        ToggleSelectCard(false);
    }

    private void ToggleSelectCard(bool isOn)
    {
        selectCardUI.gameObject.SetActive(isOn);
        // gameObject.SetActive(!isOn);
    }

    // 전체 카드 목록에서 속성에 따른 랜덤 카드 가져와 획득
    private void SetRandomCardButton(int randomCard)
    {
        RandomCardText.text = $"{GetElementKoreanString(randomCard)} 속성 카드 랜덤 획득";
        randomCardButton.onClick.AddListener(() =>
        {
            CardName cardName = CardManager.Instance.GetRandomCard((Element)(randomCard * 1000));
            DeckManager.Instance.AddCard(cardName);
            resultData.randomCard = cardName;

            rewardCardUI.PlayAddCardAnim(cardName);
            randomCardButton.gameObject.SetActive(false);
        });

        randomCardButton.gameObject.SetActive(true);
    }

    // 덱에서 속성에 따른 랜덤 카드 삭제
    private void SetRemoveCardButton(int removeCard)
    {
        RemoveCardText.text = $"{GetElementKoreanString(removeCard)} 속성 카드 랜덤 제거";
        removeCardButton.onClick.AddListener(() =>
        {
            Card card = DeckManager.Instance.GetRandomCard((Element)(removeCard * 1000));
            DeckManager.Instance.RemoveCard(card);
            resultData.removeCard = card.Name;

            rewardCardUI.PlayRemoveCardAnim(card.Name);
            removeCardButton.gameObject.SetActive(false);
        });

        removeCardButton.gameObject.SetActive(true);
    }

    private string GetElementKoreanString(int elementCode)
    {
        Element element = (Element)(elementCode * 1000);

        return element switch
        {
            Element.None => "무속성",
            Element.Ruin => "파괴",
            Element.Psychic => "사이킥",
            Element.Bio => "생체",
            _ => "전체"
        };
    }

    private string BuildResultString()
    {
        builder.Clear();

        int earnHp = resultData.earnHp;
        if (earnHp != 0)
            builder.Append($"체력 [{Mathf.Abs(earnHp)}] {(earnHp > 0 ? "회복" : "소실")}\n");

        int earnCoin = resultData.earnCoin;
        if (earnCoin != 0)
            builder.Append($"금액 [{Mathf.Abs(earnCoin)}] {(earnCoin > 0 ? "획득" : "소실")}\n");

        CardName selectedCard = resultData.selectedCard;
        if (selectedCard != 0)
            builder.Append($"[{GetKoreanCardName(selectedCard)}] 획득\n");

        CardName randomCard = resultData.randomCard;
        if (randomCard != 0)
            builder.Append($"[{GetKoreanCardName(randomCard)}] 획득\n");

        CardName removeCard = resultData.removeCard;
        if (removeCard != 0)
            builder.Append($"[{GetKoreanCardName(removeCard)}] 소실\n");

        if (builder.Length == 0)
            builder.Append("아무일도 일어나지 않았다");

        return builder.ToString();
    }

    private string GetKoreanCardName(CardName cardName)
    {
        return CardManager.Instance.GetCardData(cardName).koreanName;
    }
}

public class EventResultData
{
    public int earnHp;
    public int earnCoin;
    public CardName randomCard;
    public CardName selectedCard;
    public CardName removeCard;

    public void Reset()
    {
        earnHp = 0;
        earnCoin = 0;
        randomCard = 0;
        selectedCard = 0;
        removeCard = 0;
    }
}