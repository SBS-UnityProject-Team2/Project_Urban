using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections.Generic;

public class DiscardPanelUI : MonoBehaviour
{
    public static DiscardPanelUI Instance { get; private set; }

    [Header("UI Components")]
    [SerializeField] private GameObject panelObject;          
    [SerializeField] private Transform cardPreviewContainer;  
    [SerializeField] private TextMeshProUGUI infoText;        
    [SerializeField] private Button confirmButton;            
    [SerializeField] private Button cancelButton;             

    private List<Card> selectedCards = new List<Card>();
    
    
    private Dictionary<Card, GameObject> previewMap = new Dictionary<Card, GameObject>();

    private int maxSelectionCount;    
    private int minSelectionCount = 1;                        
    private Action<List<Card>> onDiscardCompleteCallback;     

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (panelObject != null) panelObject.SetActive(false);

        confirmButton.onClick.AddListener(OnConfirm);
        cancelButton.onClick.AddListener(OnCancel);
    }

    public void StartDiscardProcess(int count, Action<List<Card>> onComplete, int minCount = 1)
    {
        this.maxSelectionCount = count;
        this.minSelectionCount = minCount;
        this.onDiscardCompleteCallback = onComplete;

        // 데이터 및 UI 초기화
        selectedCards.Clear();
        ClearAllPreviews(); // 기존 프리뷰 싹 정리

        if (BattleManager.Instance != null && BattleManager.Instance.Player != null)
        {
            BattleManager.Instance.Player.SetDiscardMode(true);
        }

        if (panelObject != null) panelObject.SetActive(true);
        UpdateInfoText();
    }

    // 클릭한 카드만 추가하거나 제거함
    public void SelectCard(Card card)
    {
        // 1. 이미 선택된 카드 -> 제거 (Deselect)
        if (previewMap.ContainsKey(card))
        {
            RemoveCard(card);
        }
        // 2. 선택되지 않은 카드 -> 추가 (Select)
        else
        {
            if (selectedCards.Count >= maxSelectionCount)
            {
                Debug.Log("더 이상 선택할 수 없습니다.");
                return;
            }
            AddCard(card);
        }

        UpdateInfoText();
    }

    // 카드 추가 로직
    private void AddCard(Card card)
    {
        selectedCards.Add(card);

        // 프리뷰 생성
        GameObject cardClone = Instantiate(card.gameObject, cardPreviewContainer);
        
        // 딕셔너리에 등록 
        previewMap.Add(card, cardClone);

        // UI 변환 최적화 
        SetupPreviewObject(cardClone);
    }

    // 카드 제거 로직 
    private void RemoveCard(Card card)
    {
        selectedCards.Remove(card);

        // 딕셔너리에서 프리뷰 오브젝트를 찾아 제거
        if (previewMap.TryGetValue(card, out GameObject previewObj))
        {
            Destroy(previewObj);
            previewMap.Remove(card);
        }
    }

    // 모든 프리뷰 제거 (패널 닫을 때나 초기화 할 때 사용)
    private void ClearAllPreviews()
    {
        // 딕셔너리에 있는 모든 오브젝트 파괴
        foreach (var preview in previewMap.Values)
        {
            if (preview != null) Destroy(preview);
        }
        previewMap.Clear();
        
        // 혹시 모를 잔여물 처리 (안전장치)
        foreach (Transform child in cardPreviewContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetupPreviewObject(GameObject cardClone)
    {
        // 1. SpriteRenderer -> Image 변환
        if (cardClone.TryGetComponent<SpriteRenderer>(out var sr))
        {
            Sprite originalSprite = sr.sprite;
            sr.enabled = false; // 렌더러 끄기

            // Image가 없으면 추가, 있으면 가져오기
            if (!cardClone.TryGetComponent<Image>(out var img))
                img = cardClone.AddComponent<Image>();

            img.sprite = originalSprite;
            img.color = Color.white;
        }

        // 2. RectTransform 설정
        if (!cardClone.TryGetComponent<RectTransform>(out var rect))
            rect = cardClone.AddComponent<RectTransform>();

        rect.localScale = new Vector3(0.6f, 0.6f, 1f);
        rect.localRotation = Quaternion.identity;
        rect.anchoredPosition3D = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y, 0);

        // 3. LayoutElement 설정
        if (!cardClone.TryGetComponent<LayoutElement>(out var le))
            le = cardClone.AddComponent<LayoutElement>();
            
        le.preferredWidth = 200f;
        le.preferredHeight = 300f;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        // 4. CanvasGroup 설정
        if (!cardClone.TryGetComponent<CanvasGroup>(out var cg))
            cg = cardClone.AddComponent<CanvasGroup>();
            
        cg.blocksRaycasts = false;
        cg.alpha = 1f;

        // 5. 불필요한 물리 연산 제거
        if (cardClone.TryGetComponent<Collider2D>(out var col))
            col.enabled = false;

        // 6. 텍스트 표시를 위해 Card 스크립트 활성화
        if (cardClone.TryGetComponent<Card>(out var cloneScript))
            cloneScript.enabled = true;
    }

    private void UpdateInfoText()
    {
        if (infoText != null)
            infoText.text = $"버릴 카드를 선택하세요\n({selectedCards.Count}/{maxSelectionCount})";
    }

    public void OnConfirm()
    {        
        if (selectedCards.Count < minSelectionCount)
        {            
            return; 
        }

        Player player = BattleManager.Instance.Player;
        if (player != null)
        {
            foreach (Card card in selectedCards)
            {
                player.Deck.Discard(card);
            }
        }

        // 콜백 호출 (리스트 복사본 전달)
        onDiscardCompleteCallback?.Invoke(new List<Card>(selectedCards));
        ClosePanel();
    }

    private void OnCancel()
    {
        onDiscardCompleteCallback?.Invoke(null);
        ClosePanel();
    }

    private void ClosePanel()
    {
        if (panelObject != null) panelObject.SetActive(false);
        
        // 데이터 초기화
        selectedCards.Clear();
        ClearAllPreviews(); // 프리뷰 제거
        onDiscardCompleteCallback = null;
        
        if (BattleManager.Instance != null && BattleManager.Instance.Player != null)
        {
            BattleManager.Instance.Player.SetDiscardMode(false);
        }
    }
}