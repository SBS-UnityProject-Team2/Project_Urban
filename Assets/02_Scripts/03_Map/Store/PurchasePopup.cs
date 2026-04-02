using UnityEngine;
using TMPro;
using Michsky.UI.Dark;

public class PurchasePopup : MonoBehaviour
{    
    [SerializeField] private TMP_Text questionText; 
    [SerializeField] private UICard targetCardUI;

    private StoreCardUI selectedCard;
    private ModalWindowManager modalWindowManager;

    private void Awake()
    {
        modalWindowManager = GetComponent<ModalWindowManager>();
    }

    public void OpenPopup(StoreCardUI uIStoreCard, CardDataEntry cardData)
    {
        selectedCard = uIStoreCard;
        questionText.text = $"{cardData.koreanName} 구매하시겠습니까?";
        targetCardUI.Init(new DeckCard(cardData.cardName));

        // gameObject.SetActive(true);
        // modalWindowManager.ModalWindowIn();
    }

    public void OnClickConfirm()
    {
        selectedCard.Buy();
        ClosePopup();
    }

    public void OnClickCancel()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        modalWindowManager.ModalWindowOut();       
        
        Invoke(nameof(DisablePopupObject), 0.5f);
    }   

    private void DisablePopupObject()
    {
        gameObject.SetActive(false);
    }
}