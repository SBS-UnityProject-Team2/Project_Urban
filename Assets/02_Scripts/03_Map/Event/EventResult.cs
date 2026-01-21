using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Model] 선택 결과에 따른 보상 수치와 결과 스크립트를 관리합니다.
/// </summary>
[CreateAssetMenu(fileName = "EventResultData", menuName = "Event/EventResultData")]
public class EventResult : ScriptableObject
{
    [Header("1. JSON Source")]
    public TextAsset rewardTableJson;
    public TextAsset resultScriptTableJson;

    [Header("2. Data Lists")]
    [SerializeField] private List<ResultInfo> rewardList = new List<ResultInfo>();
    [SerializeField] private List<ResultScriptInfo> resultScriptList = new List<ResultScriptInfo>();

    private Dictionary<int, ResultInfo> rewardMap = new Dictionary<int, ResultInfo>();
    private Dictionary<int, ResultScriptInfo> resultScriptMap = new Dictionary<int, ResultScriptInfo>();

    public void Initialize()
    {
        rewardMap.Clear();
        resultScriptMap.Clear();

        foreach (var data in rewardList) rewardMap.TryAdd(data.ResultCode, data);
        foreach (var data in resultScriptList) resultScriptMap.TryAdd(data.ScriptCode, data);
    }

    [ContextMenu("Import From JSON")]
    public void ImportData()
    {
        rewardList = new List<ResultInfo>(JsonHelper.FromJson<ResultInfo>(rewardTableJson.text));
        resultScriptList = new List<ResultScriptInfo>(JsonHelper.FromJson<ResultScriptInfo>(resultScriptTableJson.text));
        
        Debug.Log($"[EventResult] 보상/결과 데이터 갱신 완료");
    }

    public ResultInfo GetReward(int resultCode) => rewardMap.TryGetValue(resultCode, out var data) ? data : null;
    public ResultScriptInfo GetResultScript(int scriptCode) => resultScriptMap.TryGetValue(scriptCode, out var data) ? data : null;

    [Serializable]
    public class ResultInfo
    {
        public int ResultCode;
        public float ResultHpPresent; // 현재 체력 % 변동
        public float ResultHpMaximum; // 최대 체력 % 변동
        public int ResultGold;        // 골드 변동
        public int ResultRandomCard;  // 랜덤 카드 풀 ID
        public int ResultRangeCard;   // 속성 카드 풀 ID
        public int ResultRemove;      // 카드 제거 여부
    }

    [Serializable]
    public class ResultScriptInfo
    {
        public int ScriptCode;
        public string ResultScript; // 결과 상황 설명
        public string Dialogue;     // 결과 NPC 대사
    }
}