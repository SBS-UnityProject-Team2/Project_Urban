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
        var filteredList = eventData.EventInfos.Where(info => !info.isExecuted).ToList();

        if (filteredList.Count == 0)    
            Debug.Log("Event All End");

        EventInfo eventInfo = filteredList[Random.Range(0, filteredList.Count)];
        eventInfo.isExecuted = true;

        return eventInfo;
    }

    public EventInfo GetEventInfo(int eventCode) => eventData.GetEventInfo(eventCode);
    public EventScript GetEventScript(int scriptCode) => eventData.GetEventScript(scriptCode);
    public EventChoice GetEventChoice(int choiceCode) => eventData.GetEventChoice(choiceCode);
    public EventReward GetEventReward(int resultCode) => eventData.GetEventReward(resultCode);

}
