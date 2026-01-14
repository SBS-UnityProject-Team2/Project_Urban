using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class DiscardPanelUI : MonoBehaviour
{
    public static DiscardPanelUI Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private GameObject panelObject;          // 패널 전체
    [SerializeField] private Transform cardPreviewContainer;  // Horizontal Layout Group이 달린 프리뷰 부모
    [SerializeField] private TextMeshProUGUI infoText;        // 안내 텍스트
    [SerializeField] private Button confirmButton;            // 확인(버리기 실행) 버튼
    [SerializeField] private Button cancelButton;             // 취소 버튼

    // 상태 관리 변수
    private List<Card> selectedCards = new List<Card>();      // 현재 선택한 카드 리스트
    private int maxSelectionCount;                            // 최대 선택 가능 수
    private Action<List<Card>> onDiscardCompleteCallback;     // 버리기 완료 후 카드에게 결과 알려줄 콜백

    private void Awake()
    {
        Instance = this;
        panelObject.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
    }

    // 1. 프로세스 시작
    public void StartDiscardProcess(int maxCount, Action<List<Card>> onComplete)
    {
        this.maxSelectionCount = maxCount;
        this.onDiscardCompleteCallback = onComplete;

        // 초기화
        selectedCards.Clear();
        UpdatePreviewUI();

        // 플레이어를 버리기 모드로 전환 (카드를 클릭하면 SelectCard가 호출됨)
        BattleManager.Instance.Player.SetDiscardMode(true);

        // UI 활성화
        panelObject.SetActive(true);
        UpdateInfoText();
    }

    // 2. 플레이어가 핸드에서 카드를 클릭했을 때 호출 
    public void SelectCard(Card card)
    {
        // 이미 선택된 카드라면 -> 선택 해제
        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
        }
        // 선택되지 않은 카드라면 -> 추가
        else
        {
            // 최대 개수 제한 확인
            if (selectedCards.Count >= maxSelectionCount)
            {
                // 이미 N장을 다 골랐다면 기존 것을 지우고 넣을지, 막을지 결정.
                // 여기서는 "더 이상 선택 불가"로 처리
                return;
            }
            selectedCards.Add(card);
        }

        // 프리뷰 갱신
        UpdatePreviewUI();
        UpdateInfoText();
    }

    // 선택된 카드들을 UI에 복제해서 보여주는 함수
    private void UpdatePreviewUI()
    {
        // 기존 프리뷰 삭제
        foreach (Transform child in cardPreviewContainer)
        {
            Destroy(child.gameObject);
        }

        // 현재 선택된 카드들을 복제해서 배치
        foreach (Card card in selectedCards)
        {
            // 핸드에 있는 카드 오브젝트 복사
            GameObject cardClone = Instantiate(card.gameObject, cardPreviewContainer);

            cardClone.transform.localPosition = Vector3.zero;
            cardClone.transform.localRotation = Quaternion.identity;
            cardClone.transform.localScale = Vector3.one; 

            // 복제된 카드 기능 끄기 (클릭 방지)
            CanvasGroup cg = cardClone.GetComponent<CanvasGroup>();
            if (cg == null) cg = cardClone.AddComponent<CanvasGroup>();
            cg.blocksRaycasts = false; // 마우스 입력 무시

            Card cloneScript = cardClone.GetComponent<Card>();
            if (cloneScript != null) cloneScript.enabled = false;
        }
    }

    private void UpdateInfoText()
    {
        infoText.text = $"버릴 카드를 선택하세요\n({selectedCards.Count}/{maxSelectionCount})";
    }

    // 3. 확인 버튼 클릭 시 -> 실제 버리기 수행 및 결과 전달
    private void OnConfirm()
    {        
        // 버리기 로직 
        Player player = BattleManager.Instance.Player;
        foreach (Card card in selectedCards)
        {
            player.Deck.Discard(card);
        }

        // 리스트 전달
        onDiscardCompleteCallback?.Invoke(new List<Card>(selectedCards));

        // 패널 닫기
        ClosePanel();
    }

    private void OnCancel()
    {
        ClosePanel();
    }

    private void ClosePanel()
    {
        panelObject.SetActive(false);
        selectedCards.Clear();
        onDiscardCompleteCallback = null;
        
        // 플레이어 버리기 모드 해제
        BattleManager.Instance.Player.SetDiscardMode(false);
    }
}