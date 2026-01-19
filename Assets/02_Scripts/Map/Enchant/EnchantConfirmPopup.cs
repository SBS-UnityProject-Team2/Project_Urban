using Michsky.UI.Dark;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(ModalWindowManager))]
public class EnchantConfirmPopup : MonoBehaviour
{
    [Header("Card UI Objects")]
    [SerializeField] private UICard BeforeCardUI; // 왼쪽 (강화 전)
    [SerializeField] private UICard AfterCardUI;  // 오른쪽 (강화 후)

    [Header("UI References")]
    [SerializeField] private TMP_Text AfterCardNameText; 
    [SerializeField] private CardEnchantPanel cardEnchantPanel;

    private Card selectedCard;
    private CardDataEntry currentOriginalCard; // 원본 데이터 저장용
    private CardDataEntry currentEnchantedCard; // 강화 데이터 저장용 

    public void OpenPopup(Card card)
    {        
        selectedCard = card;

        currentOriginalCard = card.Data;
        BeforeCardUI.SetCardDataEntry(currentOriginalCard); 

        currentEnchantedCard = CardManager.Instance.GetEnchantedCardData(card.Name);
        AfterCardUI.SetCardDataEntry(currentEnchantedCard);
        AfterCardNameText.text = currentEnchantedCard.koreanName;            

        GetComponent<ModalWindowManager>().ModalWindowIn();
    }

    public void ClosePopup()
    {
        GetComponent<ModalWindowManager>().ModalWindowOut();
    }

    public void OnClickEnchant()
    {
        selectedCard.Enhance();
        Debug.Log($"[강화 성공] {currentOriginalCard.cardName} -> {currentEnchantedCard.koreanName}");
        
        ClosePopup();
        cardEnchantPanel.CloseDeckDisplay();
    }
}