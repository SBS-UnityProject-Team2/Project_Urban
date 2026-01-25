using UnityEngine;
using UnityEngine.EventSystems; // UI 클릭 감지용
using System.Collections.Generic;

public class EventButton : NodeButton
{
    [SerializeField] private EventScript eventScriptSO;

    public override void OnClick()
    {
        TriggerEvent();
    }

    // 실제 이벤트 실행 함수
    private void TriggerEvent()
    {
        if (EventManager.Instance == null)
        {
            Debug.LogError("[EventButton] EventManager를 찾을 수 없습니다!");
            return;
        }

        // EventScript에서 랜덤 이벤트 코드 선택
        if (eventScriptSO == null)
        {
            Debug.LogError("[EventButton] EventScript가 연결되지 않았습니다!");
            return;
        }

        List<int> keys = eventScriptSO.GetAllEventKeys();
        if (keys != null && keys.Count > 0)
        {
            int randomEventCode = keys[Random.Range(0, keys.Count)];
            // Debug.Log($"[EventButton] 이벤트 {randomEventCode} 시작!");
            EventManager.Instance.StartEvent(randomEventCode);
        }
        else
        {
            Debug.LogWarning("[EventButton] 사용 가능한 이벤트가 없습니다!");
        }
    }
}