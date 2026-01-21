using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "Event/Event", order = 0)]
public class Event : ScriptableObject
{
    [SerializeField] List<EventInfo> eventInfos;

    public EventInfo GetEventInfo(int code)
    {
        return eventInfos.Find(info => info.code == code);
    }
}

[Serializable]
public class EventInfo
{
    public string title;
    public int stage;
    public int code;
    public int scriptCode;
    public List<int> resultCodes;
}