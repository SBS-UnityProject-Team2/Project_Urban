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
        uICard.SetCardDataEntry(cardDataEntry);

        price = cardDataEntry.price;
        cardName = cardDataEntry.cardName;
        priceText.text = $"{price}C";

        CoinController coin = PlayerManager.Instance.Coin;

        coin.OnUpdateCoin.RemoveListener(UpdatePriceText);
        coin.OnUpdateCoin.AddListener(UpdatePriceText);
            
        UpdatePriceText(coin.CurrentCoin);
    
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
        DeckManager.Instance.AddCard(cardName);   
    }

    public void AddClickHandler(UnityAction handleClick)
    {
        if(button == null) button = GetComponent<Button>();
        
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(handleClick);
    }
}