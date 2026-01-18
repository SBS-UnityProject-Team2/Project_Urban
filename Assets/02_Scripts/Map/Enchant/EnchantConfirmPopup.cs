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

    private CardDataEntry currentOriginalCard; // 원본 데이터 저장용
    private CardDataEntry currentEnchantedCard; // 강화 데이터 저장용 

    public void OpenPopup(CardDataEntry card)
    {        
        currentOriginalCard = card;

        // 1. 왼쪽: 원본 카드 표시
        BeforeCardUI.SetCardDataEntry(card); 

        // 2. 오른쪽: 강화 데이터 찾아오기
        if (CardManager.Instance != null)
        {
            currentEnchantedCard = CardManager.Instance.GetEnchantedCardData(card.cardName);
        }

        if (currentEnchantedCard != null)
        {
            // 강화 데이터가 존재하면 오른쪽 UI에 적용
            AfterCardUI.SetCardDataEntry(currentEnchantedCard);            
            
            if (AfterCardNameText != null)
            {
                AfterCardNameText.text = currentEnchantedCard.koreanName;
            }
        }
        GetComponent<ModalWindowManager>().ModalWindowIn();
    }

    public void ClosePopup()
    {
        GetComponent<ModalWindowManager>().ModalWindowOut();
    }

    public void OnClickEnchant()
    {
        if (currentEnchantedCard == null || GameManager.Instance == null) return;

        Debug.Log($"[강화 성공] {currentOriginalCard.cardName} -> {currentEnchantedCard.koreanName}");
        
        GameManager.Instance.Deck.UpgradeCard(currentOriginalCard.cardName);
    
        if (cardEnchantPanel != null && cardEnchantPanel.gameObject.activeInHierarchy)
        {
            cardEnchantPanel.OpenDeckDisplay();
        }       

        ClosePopup();
        cardEnchantPanel.CloseDeckDisplay();
    }
}