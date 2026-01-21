using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// [Model] 선택지 버튼의 텍스트와 결과 연결 정보를 관리합니다.
/// </summary>
[CreateAssetMenu(fileName = "EventChoiceData", menuName = "Event/EventChoiceData")]
public class EventChoice : ScriptableObject
{
    [Header("1. JSON Source")]
    public TextAsset choiceTableJson;

    [Header("2. Data List")]
    [SerializeField] private List<ChoiceInfo> choiceList = new List<ChoiceInfo>();
    
    private Dictionary<int, ChoiceInfo> choiceMap = new Dictionary<int, ChoiceInfo>();

    public void Initialize()
    {
        choiceMap.Clear();
        foreach (var data in choiceList) choiceMap.TryAdd(data.ChoiceCode, data);
    }

    [ContextMenu("Import From JSON")]
    public void ImportData()
    {
        choiceList = new List<ChoiceInfo>(JsonHelper.FromJson<ChoiceInfo>(choiceTableJson.text));
        Debug.Log($"[EventChoice] 선택지 데이터 갱신 완료 ({choiceList.Count}개)");
    }

    public ChoiceInfo GetChoice(int choiceCode) => choiceMap.TryGetValue(choiceCode, out var data) ? data : null;

    [Serializable]
    public class ChoiceInfo
    {
        public int ChoiceCode;
        public string ChoiceName;   // 버튼 표기 텍스트
        public string ChoiceResult; // 호버 시 예상 결과 텍스트
        public int ResultCode;      // 선택 시 적용할 보상 ID
        public int ScriptCode;      // 선택 후 출력할 결과 스크립트 ID
        public string ChoiceCondition; // 선택지 활성화 조건
    }
}