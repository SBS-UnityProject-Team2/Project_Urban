using UnityEngine;
using TMPro;
using Michsky.UI.Dark;

public class PurchasePopup : MonoBehaviour
{    
    [SerializeField] private TMP_Text questionText; 
    [SerializeField] private UICard targetCardUI;

    private StoreCardUI selectedCard;

    public void OpenPopup(StoreCardUI uIStoreCard, CardDataEntry cardData)
    {
        selectedCard = uIStoreCard;
        questionText.text = $"{cardData.koreanName} 구매하시겠습니까?";
        targetCardUI.SetCardDataEntry(cardData); 

        gameObject.SetActive(true);
        GetComponent<ModalWindowManager>().ModalWindowIn();
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
        GetComponent<ModalWindowManager>().ModalWindowOut();       
    }   
}