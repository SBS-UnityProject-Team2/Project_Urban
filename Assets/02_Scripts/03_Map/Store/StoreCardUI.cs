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
        button.onClick.RemoveListener(OnClickOpenPopup);
        button.onClick.AddListener(OnClickOpenPopup);
    }

    public void SetCardDataEntry(CardDataEntry cardDataEntry)
    {
        isSoldOut = false;
        button.interactable = true;
        
        uICard.Init(new DeckCard(cardDataEntry.cardName));

        price = cardDataEntry.price;
        cardName = cardDataEntry.cardName;
        priceText.text = $"{price}C";
        popupCardData = cardDataEntry;

        CoinController coin = PlayerManager.Instance?.Coin ?? CoinController.Fallback;

        subscribedCoin?.OnUpdateCoin.RemoveListener(UpdatePriceText);

        coin.OnUpdateCoin.AddListener(UpdatePriceText);
        subscribedCoin = coin;

        UpdatePriceText(coin.CurrentCoin);
    }

    public void BindPopup(PurchasePopup popup)
    {
        purchasePopup = popup;
    }

    private void OnDestroy()
    {
        subscribedCoin?.OnUpdateCoin.RemoveListener(UpdatePriceText);
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

        CoinController coin = subscribedCoin ?? PlayerManager.Instance?.Coin ?? CoinController.Fallback;
        coin.Decrease(price);
        DeckManager.Instance.AddCard(cardName);
    }

    public void OnClickOpenPopup()
    {
        purchasePopup.OpenPopup(this, popupCardData);
    }
}