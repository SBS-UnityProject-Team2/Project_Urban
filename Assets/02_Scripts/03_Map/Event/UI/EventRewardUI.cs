using System.Text;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
    [SerializeField] private Button selectArtifactButton;

    [Header("UI Reference")]
    [SerializeField] private SelectCardUI selectCardUI;
    [SerializeField] private EventRewardCardUI rewardCardUI;
    // [SerializeField] private EventRewardArtifactUI rewardArtifactUI;

    // EventResult에 데이터 전달용
    private readonly EventResultData resultData = new();
    private readonly UniTaskCompletionSource<(int, string)> resultCompletionSource = new();

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

    public void SetReward(EventRewardData eventRewardData)
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

        confirmButton.onClick.AddListener(() => resultCompletionSource.TrySetResult((eventRewardData.scriptCode, BuildResultString())));
    }

    public async UniTask<(int, string)> GetResult()
    {
        return await resultCompletionSource.Task;
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
            selectCardUI.gameObject.SetActive(true);

            selectCardUI.Init();
            selectCardUI.SetSelectCards(cardNames, HandleSelectCard, () => selectCardUI.gameObject.SetActive(false));
        });

        selectCardButton.gameObject.SetActive(true);
    }

    private void HandleSelectCard(CardName cardName)
    {
        resultData.selectedCard = cardName;
        selectCardButton.gameObject.SetActive(false);
        selectCardUI.gameObject.SetActive(false);
    }


    // 전체 카드 목록에서 속성에 따른 랜덤 카드 가져와 획득
    private void SetRandomCardButton(int randomCard)
    {
        RandomCardText.text = $"{GetElementKoreanString(randomCard)} 속성 카드 랜덤 획득";
        randomCardButton.onClick.AddListener(() =>
        {
            CardName cardName = CardManager.Instance.GetRandomCard((ElementType)(randomCard * 1000));
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
            DeckCard card = DeckManager.Instance.GetRandomCard((ElementType)(removeCard * 1000));
            DeckManager.Instance.RemoveCard(card);
            resultData.removeCard = card.Name;

            rewardCardUI.PlayRemoveCardAnim(card.Name);
            removeCardButton.gameObject.SetActive(false);
        });

        removeCardButton.gameObject.SetActive(true);
    }

    private string GetElementKoreanString(int elementCode)
    {
        ElementType element = (ElementType)(elementCode * 1000);

        return element switch
        {
            ElementType.None => "무속성",
            ElementType.Ruin => "파괴",
            ElementType.Psychic => "사이킥",
            ElementType.Bio => "생체",
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