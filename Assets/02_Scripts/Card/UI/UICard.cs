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
        Debug.Log($"[UICard] SetCardDataEntry 시작: {data.koreanName} ({data.cardName})");
        
        cardName = data.cardName;
        cardImage.sprite = data.cardSprite;
        cardTitle.text = data.koreanName;
        cardDesc.text = data.description;       
        cardCost.text = data.cost.ToString();
        
        Debug.Log($"[UICard] 데이터 설정 완료 - Title: {cardTitle.text}, Cost: {cardCost.text}, Sprite: {(cardImage.sprite != null ? cardImage.sprite.name : "null")}");
    }
}