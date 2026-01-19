using UnityEngine;
using TMPro;
using Michsky.UI.Dark;

[RequireComponent(typeof(ModalWindowManager))]
public class RemoveConfirmPopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UICard targetCardUI; 
    [SerializeField] private GameObject Panel_CardRemovePanel;

    private Card targetCard;    

    public void OpenPopup(Card card)
    {
        targetCard = card;
        targetCardUI.SetCardDataEntry(card.Data);
        GetComponent<ModalWindowManager>().ModalWindowIn();
    }

    public void OnClickRemove()
    {  
        GameManager.Instance.Deck.RemoveCard(targetCard);
        ClosePopup();
        Panel_CardRemovePanel.SetActive(false);
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