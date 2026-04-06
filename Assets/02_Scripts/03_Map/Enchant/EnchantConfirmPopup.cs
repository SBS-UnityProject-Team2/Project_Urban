using Michsky.UI.Dark;
using System.Collections;
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
    private CardDataEntry currentOriginalCard; 
    private CardDataEntry currentEnchantedCard; 
    private ModalWindowManager modalWindowManager;
    private bool isReadyForUserConfirm;
    private bool isEnchantProcessing;

    private void Awake()
    {
        modalWindowManager = GetComponent<ModalWindowManager>();
    }

    public void OpenPopup(DeckCard cardInstance)
    {
        if (!gameObject.activeSelf) 
            gameObject.SetActive(true);

        isReadyForUserConfirm = false;
        isEnchantProcessing = false;

        selectedCard = cardInstance;

        currentOriginalCard = cardInstance.CardData;
        BeforeCardUI.Init(cardInstance);

        currentEnchantedCard = CardManager.Instance.GetEnchantCardData(cardInstance.Name);
        Sprite enchantSprite = CardManager.Instance.GetEnchantCardImage(cardInstance.Name);
        AfterCardUI.Init(currentEnchantedCard, enchantSprite);
        AfterCardNameText.text = currentEnchantedCard.koreanName;

        StartCoroutine(PlayModalInNextFrame());
    }

    private IEnumerator PlayModalInNextFrame()
    {
        yield return null; 
        
        modalWindowManager.ModalWindowIn();

        yield return null;
        isReadyForUserConfirm = true;
    }

    public void ClosePopup()
    {
        isReadyForUserConfirm = false;

        modalWindowManager.ModalWindowOut();
    }

    public void OnClickEnchant()
    {
        if (!isReadyForUserConfirm || isEnchantProcessing || selectedCard == null)
            return;

        isEnchantProcessing = true;
        isReadyForUserConfirm = false;

        DeckManager.Instance.Enchant(selectedCard.Id);
        Debug.Log($"[강화 성공] {currentOriginalCard.cardName} -> {currentEnchantedCard.koreanName}");
        
        // 1. 팝업창 닫기 애니메이션 시작
        modalWindowManager.ModalWindowOut();
        
        // 2. 애니메이션이 끝날 시간을 벌어준 뒤 부모 패널 끄기
        StartCoroutine(ClosePanelAfterDelay());
    }

    private IEnumerator ClosePanelAfterDelay()
    {
        // 팝업이 스르륵 사라질 때까지 0.5초 대기
        yield return new WaitForSeconds(0.5f);
        cardEnchantPanel.CloseDeckDisplay();
    }
}