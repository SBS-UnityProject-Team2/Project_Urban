using UnityEngine;
using TMPro;
using Michsky.UI.Dark;

[RequireComponent(typeof(ModalWindowManager))]
public class RemoveConfirmPopup : MonoBehaviour
{
    [Header("UI 연결")]
    [SerializeField] private UICard RemoveCardUI;    // 제거 대상 카드
    [SerializeField] private CardRemovePanel cardRemovePanel;
    private DeckCard targetCard;
    private ModalWindowManager modalWindowManager;
    private CardDataEntry targetCardUI;

    private void Awake()
    {
        modalWindowManager = GetComponent<ModalWindowManager>();
    }

    public void OpenPopup(DeckCard cardInstance)
    {   
        gameObject.SetActive(true);
        targetCard = cardInstance;
        targetCardUI = cardInstance.CardData;

        RemoveCardUI.Init(cardInstance);

        modalWindowManager.ModalWindowIn();
    }

    public void OnClickRemove()
    {  
        DeckManager.Instance.Remove(targetCard.Id);
        MapManager.Instance.SetCanRemove(false);
        cardRemovePanel.UpdateOpenButton();
        Debug.Log($"[카드 제거] {targetCardUI.koreanName} 제거 완료");

        ClosePopup();
        cardRemovePanel.CloseDeckDisplay();
    }

    public void OnClickCancel()
    {
        ClosePopup();
    }

    private void ClosePopup()
    {
        modalWindowManager.ModalWindowOut();
        Invoke(nameof(DisablePopup), 0.5f);
    }

    private void DisablePopup()
    {
        gameObject.SetActive(false);
    }
}