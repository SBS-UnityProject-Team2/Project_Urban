using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Model] 이벤트의 흐름(EventTable)과 대사/이미지(EventScript) 데이터를 관리하는 저장소입니다.
/// </summary>
[CreateAssetMenu(fileName = "EventScriptData", menuName = "Event/EventScriptData")]
public class EventScript : ScriptableObject
{
    [Header("1. JSON Source")]
    public TextAsset eventTableJson;
    public TextAsset scriptTableJson;

    [Header("2. Data Lists (Serialized)")]
    [SerializeField] private List<EventInfo> eventList = new List<EventInfo>();
    [SerializeField] private List<ScriptInfo> scriptList = new List<ScriptInfo>();

    // 런타임 최적화를 위한 딕셔너리 (ID 검색 속도: O(1))
    private Dictionary<int, EventInfo> eventMap = new Dictionary<int, EventInfo>();
    private Dictionary<int, ScriptInfo> scriptMap = new Dictionary<int, ScriptInfo>();

    /// <summary>
    /// 게임 시작 시 리스트를 딕셔너리로 변환하여 검색 속도를 최적화합니다.
    /// </summary>
    public void Initialize()
    {
        eventMap.Clear();
        scriptMap.Clear();

        // 빠른 루프를 위해 foreach 대신 for문 사용 가능하나, 초기화 1회성이므로 가독성 유지
        foreach (var data in eventList) eventMap.TryAdd(data.EventCode, data);
        foreach (var data in scriptList) scriptMap.TryAdd(data.ScriptCode, data);
    }

    /// <summary>
    /// 에디터 전용: JSON 데이터를 리스트로 파싱하여 저장합니다.
    /// </summary>
    [ContextMenu("Import From JSON")]
    public void ImportData()
    {
        // Null 체크 제거: 개발자가 파일을 연결했다고 가정
        eventList = new List<EventInfo>(JsonHelper.FromJson<EventInfo>(eventTableJson.text));
        scriptList = new List<ScriptInfo>(JsonHelper.FromJson<ScriptInfo>(scriptTableJson.text));
        
        Debug.Log($"[EventScript] 데이터 갱신 완료 (Event: {eventList.Count}, Script: {scriptList.Count})");
    }

    // 데이터 조회 (Dictionary 사용으로 고속 접근)
    public EventInfo GetEvent(int eventCode) => eventMap.TryGetValue(eventCode, out var data) ? data : null;
    public ScriptInfo GetScript(int scriptCode) => scriptMap.TryGetValue(scriptCode, out var data) ? data : null;
    public List<int> GetAllEventKeys() => new List<int>(eventMap.Keys);

    // --- 데이터 구조체 ---
    [Serializable]
    public class EventInfo
    {
        public int EventCode;
        public string EventName;
        public int EventScript;     // 시작 시 출력할 스크립트 ID
        public int EventChoice1;    // 선택지 ID 1~3
        public int EventChoice2;
        public int EventChoice3;
    }

    [Serializable]
    public class ScriptInfo
    {
        public int ScriptCode;
        public string EventScript;  // 상황 설명 텍스트
        public string Dialogue;     // NPC 대사
        public string Illustration; // 이미지 리소스 이름
    }
}