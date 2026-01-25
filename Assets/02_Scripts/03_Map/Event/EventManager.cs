using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// ╔════════════════════════════════════════════════════════════════════╗
/// ║                     EVENT MANAGER (중앙 컨트롤러)                    ║
/// ╚════════════════════════════════════════════════════════════════════╝
/// 
/// 📋 역할:
///   • 이벤트 데이터 관리 (EventScript, EventChoice, EventResult 캐싱)
///   • 이벤트 흐름 제어 (시작 → 선택지 표시 → 보상 적용 → 종료)
///   • 선택지 조건 검증 (덱 카드 필요 조건 확인)
///   • 보상 시스템 통합 (HP, 골드, 카드 보상)
///   • UI ↔ 게임로직 중계층
/// 
/// ⚡ 메모리 최적화:
///   ✓ Singleton 패턴: 씬 전환 후에도 유지 (이벤트 데이터 캐시 재활용)
///   ✓ GameObject.Find() 사용: FindObjectOfType 제거 (성능 개선)
///   ✓ 코루틴 정리: 새 이벤트 시작 시 기존 코루틴 중단 (메모리 누수 방지)
///   ✓ UI 참조 캐싱: SetUI()로 한 번만 할당 후 재사용 (GC Alloc 최소화)
///   ✓ 조건 검사 최적화: Any() 사용 (첫 매칭 즉시 종료)
/// 
/// 📊 데이터 흐름:
///   EventButton.OnClick()
///       ↓
///   EventManager.StartEvent(eventCode)
///       ├─ EventScript에서 이벤트 정보 조회
///       ├─ EventUI에 대사 + 일러스트 표시
///       └─ 타이핑 완료 후 ShowChoices()
///           ├─ EventChoice에서 선택지 로드
///           ├─ 각 선택지마다 CheckCondition()
///           └─ 조건 충족 여부에 따라 버튼 활성화
///               ↓ (사용자 클릭)
///               OnChoiceSelected()
///               ├─ EventResult에서 보상 로드
///               ├─ ApplyRewardLogic()으로 보상 적용
///               └─ 결과 스크립트 표시
/// </summary>
public class EventManager : Singleton<EventManager>
{
    [Header("Data Assets")]
    [SerializeField] private EventScript eventScriptSO;
    [SerializeField] private EventChoice eventChoiceSO;
    [SerializeField] private EventResult eventResultSO;

    [Header("UI Reference")]
    [SerializeField] private EventUI eventUI;
    [SerializeField] private GameObject eventPanel;
    [SerializeField] private EventSelectCardReward cardRewardPanel; // 카드 선택 패널
    [SerializeField] private CardData cardDataSO;

    // =================================================================
    // [초기화 & 참조 관리]
    // =================================================================

    public void SetUI(EventUI ui, GameObject panel)
    {
        this.eventUI = ui;
        this.eventPanel = panel;
        this.eventPanel.SetActive(false);
    }

   private void EnsureUIReferences()
    {
        EventUI[] foundUIs = FindObjectsByType<EventUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        eventUI = foundUIs[0];
        eventPanel = eventUI.gameObject;
    }

    protected override void Awake()
    {
        base.Awake();
        
        // [성능] Dictionary 캐싱으로 JSON 재파싱 방지
        eventScriptSO.Initialize();
        eventChoiceSO.Initialize();
        eventResultSO.Initialize();
    }

    private void Start()
    {
    }

    // =================================================================
    // [1. 이벤트 시작]
    // =================================================================

    public void StartRandomEvent()
    {
        List<int> keys = eventScriptSO.GetAllEventKeys();
        int randomCode = keys[Random.Range(0, keys.Count)];
        StartEvent(randomCode);
    }

    public void StartEvent(int eventCode)
    {
        // ★ 시작 전 무조건 참조 복구 시도
        EnsureUIReferences();

        StopAllCoroutines();

        // 새로운 플로우: UI가 eventCode를 받아 스스로 조회/표시
        if (!eventUI.enabled) eventUI.enabled = true;
        eventUI.BeginEvent(eventCode);
    }

    // =================================================================
    // [2. 선택지 표시] - EventUI가 버튼에 직접 할당하므로 더 이상 필요 없음
    // =================================================================

    // =================================================================
    // [3. 조건 검증] - ConditionCheck 유틸리티로 위임
    // =================================================================

    public bool CheckCondition(ConditionType condition)
    {
        return ConditionCheck.CheckCondition(condition);
    }

    // =================================================================
    // [4. 데이터 공급]

            // =============================================================
            // [Public Getters] : 데이터 공급 전용
            // =============================================================
            public EventScript.EventInfo GetEventInfo(int eventCode) => eventScriptSO?.GetEvent(eventCode);
            public EventScript.ScriptInfo GetScriptInfo(int scriptCode) => eventScriptSO?.GetScript(scriptCode);
            public List<int> GetAllEventKeys() => eventScriptSO != null ? eventScriptSO.GetAllEventKeys() : new List<int>();

            public EventChoice.ChoiceInfo GetChoiceInfo(int choiceCode) => eventChoiceSO?.GetChoice(choiceCode);

            public EventResult.ResultInfo GetRewardInfo(int resultCode) => eventResultSO?.GetReward(resultCode);
            public EventResult.ResultScriptInfo GetResultScriptInfo(int scriptCode) => eventResultSO?.GetScript(scriptCode);
    // =================================================================

    private IEnumerator EndEventDelay()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        eventPanel.SetActive(false);
    }

    // =================================================================
    // [Public Getter for CardData]
    // =================================================================
    public CardData GetCardData() => cardDataSO;
    public EventSelectCardReward GetCardRewardPanel() => cardRewardPanel;
}
