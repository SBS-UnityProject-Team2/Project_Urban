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
    
    public void SetCardDataEntry(CardDataEntry data)
    {
        cardName = data.cardName;
        cardImage.sprite = data.cardSprite;
        cardTitle.text = data.koreanName;
        cardDesc.text = data.description;       
        cardCost.text = data.cost.ToString();    
    }
}