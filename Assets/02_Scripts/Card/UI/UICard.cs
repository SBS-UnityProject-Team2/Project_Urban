using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UICard : MonoBehaviour
{
    [SerializeField] private TMP_Text cardTitle;
    [SerializeField] private TMP_Text cardDesc;
    [SerializeField] private TMP_Text cardCost;
    [SerializeField] private Image cardImage;
    
    private CardName cardName;
    public CardName CardName => cardName;

    public void SetCardName(CardName cardName)
    {
        this.cardName = cardName;

        CardDataEntry data = CardManager.Instance.GetCardData(cardName);
        Sprite cardSprite = CardManager.Instance.GetCardImage(cardName);
        ApplyCardData(data, cardSprite);
    }
    
    public void SetCardDataEntry(CardDataEntry data)
    {
        cardName = data.cardName;
        Sprite cardSprite = CardManager.Instance.GetCardImage(data.cardName);
        ApplyCardData(data, cardSprite);
    }

    private void ApplyCardData(CardDataEntry data, Sprite cardSprite)
    {
        cardImage.sprite = cardSprite;
        cardTitle.text = data.koreanName;
        cardDesc.text = data.description;
        cardCost.text = data.cost.ToString();
    }
}