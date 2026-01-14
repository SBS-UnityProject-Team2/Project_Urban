using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DiscardPanelUI : MonoBehaviour
{
    public static DiscardPanelUI Instance { get; private set; }

    [Header("UI Settings")]
    [SerializeField] private GameObject panelObject;
    [SerializeField] private Transform cardPreviewContainer; 
    
    [SerializeField] private TextMeshProUGUI infoText;
    [SerializeField] private Button confirmButton;
    //[SerializeField] private Button cancelButton;

    private Card targetCard;    
    private Action<Card> onCardSelectedCallback; 

    private void Awake()
    {
        Instance = this;
        panelObject.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirm);
        //cancelButton.onClick.AddListener(OnCancel);
    }

    public void StartSelectionProcess(Action<Card> onSelected)
    {
        this.onCardSelectedCallback = onSelected;
        BattleManager.Instance.Player.SetDiscardMode(true);
    }

    public void SelectCard(Card card)
    {
        targetCard = card;
        panelObject.SetActive(true);
        infoText.text = $"{card.Name} 카드를\n선택하시겠습니까?";

        ShowCardPreview(card);      // 선택한 카드 프리펩 복사해서 패널에 띄우기
    }

    private void ShowCardPreview(Card originalCard)
    {
        // 1. 기존 프리뷰 제거
        foreach (Transform child in cardPreviewContainer)
        {
            Destroy(child.gameObject);
        }

        // 2. 카드 복제
        GameObject cardClone = Instantiate(originalCard.gameObject, cardPreviewContainer);

        // 3. 위치 및 크기 초기화
        cardClone.transform.localPosition = Vector3.zero;
        cardClone.transform.localRotation = Quaternion.identity;
        cardClone.transform.localScale = Vector3.one;

        // 4. 버튼 기능 비활성화         
        CanvasGroup cg = cardClone.GetComponent<CanvasGroup>();
        if (cg == null) 
        {
            cg = cardClone.AddComponent<CanvasGroup>();
        }
        
        cg.blocksRaycasts = false; 

        // 5. (안전장치) Card 스크립트 비활성화
        // 삭제(Destroy)하지 않고, 단순히 꺼둠(enabled = false)으로써 Update문 등이 도는 것을 방지합니다.
        //Card cloneScript = cardClone.GetComponent<Card>();
        //if (cloneScript != null)
        //{
           // cloneScript.enabled = false; 
        //}
    }

    private void OnConfirm()
    {
        if (targetCard != null)
        {
            onCardSelectedCallback?.Invoke(targetCard);
        }
        ClosePanel();
    }

    private void OnCancel()
    {
        ClosePanel();
    }

    private void ClosePanel()
    {
        panelObject.SetActive(false);
        targetCard = null;
        onCardSelectedCallback = null;
        BattleManager.Instance.Player.SetDiscardMode(false);
    }
}