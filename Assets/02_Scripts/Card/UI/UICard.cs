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

    public void Init(DeckCard cardInstance)
    {
        CardDataEntry cardData = cardInstance.CardData;
        
        cardTitle.text = $"{cardData.koreanName}";
        cardDesc.text = CardManager.Instance.GetDescription(cardInstance.Name);
        cardCost.text = $"{cardData.cost}";
        cardImage.sprite = CardManager.Instance.GetCardImage(cardInstance.Name);
    }
}