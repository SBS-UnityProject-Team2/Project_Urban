using UnityEngine;

public class EventManager : SceneSingleton<EventManager>
{
    [SerializeField] private Event eventData;
    [SerializeField] private EventScript eventScriptData;
    [SerializeField] private EventChoice eventChoiceData;
    [SerializeField] private EventResult eventResultData;

    public string GetResultString(int resultCode)
    {
        return eventResultData.GetResult(resultCode).desc;
    }
}