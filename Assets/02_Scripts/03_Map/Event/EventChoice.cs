using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventChoiceData", menuName = "Event/EventChoiceData")]
public class EventChoice : ScriptableObject
{
    [Header("JSON File")]
    public TextAsset choiceTableJson;

    [Header("Data List (Read Only)")]
    [SerializeField] private List<ChoiceInfo> choiceList = new List<ChoiceInfo>();

    private Dictionary<int, ChoiceInfo> choiceMap = new Dictionary<int, ChoiceInfo>();

    public void Initialize()
    {
        choiceMap.Clear();
        foreach (var data in choiceList)
        {
            if (!choiceMap.ContainsKey(data.ChoiceCode)) choiceMap.Add(data.ChoiceCode, data);
        }
    }

    [ContextMenu("Import From JSON")]
    public void ImportData()
    {
        choiceList = new List<ChoiceInfo>(JsonHelper.FromJson<ChoiceInfo>(choiceTableJson.text));
        Debug.Log($"[EventChoice] {choiceList.Count}개의 선택지 데이터 로드 완료!");
    }

    public ChoiceInfo GetChoice(int code) => choiceMap.TryGetValue(code, out var data) ? data : null;

    [Serializable]
    public class ChoiceInfo
    {
        public int ChoiceCode;
        public int CascadeEvent;
        public string ChoiceName;
        
        // JSON에서는 숫자로 들어옴 (0, 1, 2...)
        public int ChoiceCondition; 
        
        public string ChoiceResult;
        public int ResultCode;
        public int ScriptCode;

        public ConditionType ConditionEnum => (ConditionType)ChoiceCondition;
    }
}