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
    [SerializeField] private EventData eventData;

    [Header("Event Value Settings")]
    [SerializeField] private float coinRatio = 0.2f;

    public float CoinRatio => coinRatio;

    public EventInfo GetRandomEvent()
    {
        var filteredList = eventData.EventInfos.Where(info => !info.isExecuted).ToList();
        
        if (filteredList.Count == 0)    
            Debug.Log("Event All End");

        EventInfo eventInfo = filteredList[Random.Range(0, filteredList.Count())];
        eventInfo.isExecuted = true;

        return eventInfo;
    }

    public EventInfo GetEventInfo(int eventCode) => eventData.GetEventInfo(eventCode);
    public EventScript GetEventScript(int scriptCode) => eventData.GetEventScript(scriptCode);
    public EventChoice GetEventChoice(int choiceCode) => eventData.GetEventChoice(choiceCode);
    public EventReward GetEventReward(int resultCode) => eventData.GetEventReward(resultCode);
    public EventResult GetEventResult(int scriptCode) => eventData.GetEventResult(scriptCode);
    public RangeCardPool GetRangeCardPool(int poolCode) => eventData.GetRangeCardPool(poolCode);

}
