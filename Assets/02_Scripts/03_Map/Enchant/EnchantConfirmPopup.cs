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

    private DeckCard selectedCard;
    private CardDataEntry currentOriginalCard; // 원본 데이터 저장용
    private CardDataEntry currentEnchantedCard; // 강화 데이터 저장용
    private ModalWindowManager modalWindowManager;

    private void Awake()
    {
        modalWindowManager = GetComponent<ModalWindowManager>();
    }

    public void OpenPopup(DeckCard cardInstance)
    {        
        selectedCard = cardInstance;

        currentOriginalCard = cardInstance.CardData;
        BeforeCardUI.Init(cardInstance);

        // 현재 데이터 구조에서는 강화 전/후가 별도 데이터로 분리되어 있지 않아 미리보기는 동일 카드 기준입니다.
        currentEnchantedCard = currentOriginalCard;
        AfterCardUI.Init(cardInstance);
        AfterCardNameText.text = currentEnchantedCard.koreanName;

        modalWindowManager.ModalWindowIn();
    }

    public void ClosePopup()
    {
        modalWindowManager.ModalWindowOut();
    }

    public void OnClickEnchant()
    {
        DeckManager.Instance.Enchant(selectedCard.Id);
        Debug.Log($"[강화 성공] {currentOriginalCard.cardName} -> {currentEnchantedCard.koreanName}");
        
        ClosePopup();
        cardEnchantPanel.CloseDeckDisplay();
    }
}