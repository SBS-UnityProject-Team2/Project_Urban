using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StoreCardUI : MonoBehaviour, IClickable
{
    [SerializeField] private UICard uICard;
    [SerializeField] private TMP_Text priceText;

    private int price;
    private bool isSoldOut;
    private Button button;
    private CardName cardName;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetCardDataEntry(CardDataEntry cardDataEntry)
    {
        // 1. 데이터가 비어있으면 중단
        if (cardDataEntry == null)
        {
            Debug.LogError("StoreCardUI: 받은 카드 데이터가 Null입니다!");
            return;
        }

        // 2. UI 카드 비주얼 업데이트 (여기도 에러날 수 있으니 체크)
        if (uICard != null)
        {
            uICard.SetCardDataEntry(cardDataEntry);
        }

        price = cardDataEntry.price;
        cardName = cardDataEntry.cardName;
        
        // 3. 가격 텍스트 업데이트
        if (priceText != null)
        {
            priceText.text = $"{price}C";
        }

        if (GameManager.Instance != null && GameManager.Instance.Coin != null)
        {
            // 이벤트 중복 등록 방지 
            GameManager.Instance.Coin.OnUpdateCoin.RemoveListener(UpdatePriceText);
            GameManager.Instance.Coin.OnUpdateCoin.AddListener(UpdatePriceText);
            
            UpdatePriceText(GameManager.Instance.Coin.CurrentCoin);
        }
        else
        {
            // GameManager가 없을 때를 대비한 로그 (게임 멈춤 방지)
            Debug.LogWarning("현재 보유 코인 연결안됨");
            
            // 기본적으로 구매 가능 상태로 둠
            if (button != null) button.interactable = true;
        }
    }

    private void UpdatePriceText(int curCoin)
    {
        if (isSoldOut || button == null || priceText == null) return;

        if (price > curCoin)
        {
            priceText.color = Color.red;
            button.interactable = false;
        }
        else
        {
            // 돈이 충분해지면 다시 하얀색으로 (필요하다면 추가)
            priceText.color = Color.black;
            button.interactable = true;
        }
    }

    public void Buy()
    {
        if (isSoldOut) return;

        isSoldOut = true;
        if(button != null) button.interactable = false;
        if(priceText != null) priceText.text = "Sold Out";        

        if (GameManager.Instance != null)
        {
            GameManager.Instance.UseCoin(price);
            GameManager.Instance.Deck.AddCard(cardName);
        }
    }

    public void AddClickHandler(UnityAction handleClick)
    {
        if(button == null) button = GetComponent<Button>();
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(handleClick);
    }
}