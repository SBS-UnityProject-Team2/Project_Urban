using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StoreCardUI : MonoBehaviour
{
    [SerializeField] private UICard uICard;
    [SerializeField] private TMP_Text priceText;

    private int price;
    private bool isSoldOut;
    private Button button;
    private CardName cardName;
    private CoinController subscribedCoin;
    private PurchasePopup purchasePopup;
    private CardDataEntry popupCardData;

    private void Awake()
    {
        button = GetComponent<Button>();
        subscribedCoin = PlayerManager.Instance.Coin;
    }

    public void SetCardDataEntry(CardDataEntry cardDataEntry)
    {
        isSoldOut = false;
        button.interactable = true;

        // uICard.SetCardDataEntry(cardDataEntry);

        price = cardDataEntry.price;
        cardName = cardDataEntry.cardName;
        priceText.text = $"{price}C";

        CoinController coin = PlayerManager.Instance.Coin;

        coin.OnUpdateCoin.RemoveListener(UpdatePriceText);
        coin.OnUpdateCoin.AddListener(UpdatePriceText);
        subscribedCoin = coin;
            
        UpdatePriceText(coin.CurrentCoin);
    
    }

    private void OnDestroy()
    {
        subscribedCoin.OnUpdateCoin.RemoveListener(UpdatePriceText);
        button.onClick.RemoveListener(OnClickOpenPopup);
    }

    private void UpdatePriceText(int curCoin)
    {
        if (isSoldOut) return;

        if (price > curCoin)
        {
            priceText.color = Color.red;
            button.interactable = false;
        }
        else
        {
            priceText.color = Color.white;
            button.interactable = true;
        }
    }

    public void Buy()
    {
        if (isSoldOut) return;

        isSoldOut = true;
        button.interactable = false;
        priceText.text = "Sold Out";    

        PlayerManager.Instance.Coin.Increase(price);
        // DeckManager.Instance.AddCard(cardName);      임시 비활성화(덱연결안됨)
    }

    public void BindPopup(PurchasePopup popup, CardDataEntry cardData)
    {
        purchasePopup = popup;
        popupCardData = cardData;

        button.onClick.RemoveListener(OnClickOpenPopup);
        button.onClick.AddListener(OnClickOpenPopup);
    }

    private void OnClickOpenPopup()
    {
        purchasePopup.OpenPopup(this, popupCardData);
    }
}