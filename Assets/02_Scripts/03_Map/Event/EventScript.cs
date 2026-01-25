using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Data Model]
/// JSON에서 파싱된 이벤트 진행 흐름(ID, 연결된 선택지)과 대사 데이터를 관리합니다.
/// </summary>
[CreateAssetMenu(fileName = "EventScriptData", menuName = "Event/EventScriptData")]
public class EventScript : ScriptableObject
{
    public TextAsset eventTableJson;
    public TextAsset scriptTableJson;

    [SerializeField] private List<EventInfo> eventList = new List<EventInfo>();
    [SerializeField] private List<ScriptInfo> scriptList = new List<ScriptInfo>();

    private Dictionary<int, EventInfo> eventMap = new Dictionary<int, EventInfo>();
    private Dictionary<int, ScriptInfo> scriptMap = new Dictionary<int, ScriptInfo>();

    public void Initialize()
    {
        eventMap.Clear();
        scriptMap.Clear();
        foreach (var d in eventList) eventMap.TryAdd(d.EventCode, d);
        foreach (var d in scriptList) scriptMap.TryAdd(d.ScriptCode, d);
    }

    [ContextMenu("Import From JSON")]
    public void ImportData()
    {
        eventList = new List<EventInfo>(JsonHelper.FromJson<EventInfo>(eventTableJson.text));
        scriptList = new List<ScriptInfo>(JsonHelper.FromJson<ScriptInfo>(scriptTableJson.text));
        Debug.Log($"[EventScript] 데이터 로드 완료 (E:{eventList.Count}, S:{scriptList.Count})");
    }

    public EventInfo GetEvent(int code) => eventMap.TryGetValue(code, out var d) ? d : null;
    public ScriptInfo GetScript(int code) => scriptMap.TryGetValue(code, out var d) ? d : null;
    public List<int> GetAllEventKeys() => new List<int>(eventMap.Keys);

    [Serializable]
    public class EventInfo
    {
        public int EventCode;
        public string EventName;
        public int EventScript; 
        public int EventChoice1;
        public int EventChoice2;
        public int EventChoice3;
    }

    [Serializable]
    public class ScriptInfo
    {
        public int ScriptCode;
        public string EventScript;
        public string Dialogue;
        public string Illustration;
    }
}