using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventManager : Singleton<EventManager>
{
    [Header("Data Assets")]
    [SerializeField] private EventData eventData;

    [Header("Event Value Settings")]
    [SerializeField] private float coinRatio = 0.2f;

    public float CoinRatio => coinRatio;

    public EventInfo GetRandomEvent()
    {
        if (eventData == null)
        {
            Debug.LogError("EventData가 연결되지 않았습니다.");
            return null;
        }

        // 데모버전 고정 이벤트(16번)만 반환
        EventInfo eventInfo = eventData.GetEventInfo(16);

        if (eventInfo == null)
        {
            Debug.LogWarning("eventCode 16 이벤트를 찾지 못해 첫 번째 이벤트로 대체합니다.");

            if (eventData.EventInfos != null && eventData.EventInfos.Count > 0)
                eventInfo = eventData.EventInfos[0];
        }

        if (eventInfo == null)
        {
            Debug.LogError("사용 가능한 이벤트가 없습니다.");
            return null;
        }

        /*
        var filteredList = eventData.EventInfos.Where(info => !info.isExecuted).ToList();

        if (filteredList.Count == 0)
            Debug.Log("Event All End");

        EventInfo eventInfo = filteredList[Random.Range(0, filteredList.Count)];
        */

        eventInfo.isExecuted = true;

        return eventInfo;
    }

    public EventInfo GetEventInfo(int eventCode) => eventData.GetEventInfo(eventCode);
    public EventScript GetEventScript(int scriptCode) => eventData.GetEventScript(scriptCode);
    public EventChoice GetEventChoice(int choiceCode) => eventData.GetEventChoice(choiceCode);
    public EventReward GetEventReward(int resultCode) => eventData.GetEventReward(resultCode);

}
