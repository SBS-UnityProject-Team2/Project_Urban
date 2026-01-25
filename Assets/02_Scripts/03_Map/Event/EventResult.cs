using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventResultData", menuName = "Event/EventResultData")]
public class EventResult : ScriptableObject
{
    [Header("JSON File")]
    public TextAsset rewardTableJson;
    public TextAsset resultScriptTableJson;

    [Header("Card Pool Settings (카드 풀 관리)")]
    [Tooltip("카드 풀 배열: ResultRangeCard 값에 따라 3장씩 분류")]
    [SerializeField] private List<RangeCardPool> rangeCardPools = new List<RangeCardPool>();

    [Header("Data Lists")]
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
        Debug.Log("[EventResult] 데이터 임포트 완료");
    }

    public ResultInfo GetReward(int code) => rewardMap.TryGetValue(code, out var data) ? data : null;
    public ResultScriptInfo GetScript(int code) => resultScriptMap.TryGetValue(code, out var data) ? data : null;

    /// <summary>
    /// ResultRangeCard 값으로 카드 풀 조회 (1-based index)
    /// </summary>
    public RangeCardPool GetCardPool(int poolIndex)
    {
        if (poolIndex <= 0 || poolIndex > rangeCardPools.Count)
            return null;
        
        return rangeCardPools[poolIndex - 1]; // 1-based -> 0-based
    }

    [Serializable]
    public class RangeCardPool
    {
        [Tooltip("카드 풀 이름 (ex: Choice01 카드 풀)")]
        public string poolName;
        
        [Tooltip("플레이어가 선택할 3장의 카드")]
        public CardName poolCard1;
        public CardName poolCard2;
        public CardName poolCard3;

        public List<CardName> GetCardList()
        {
            List<CardName> cards = new List<CardName>();
            if (poolCard1 != (CardName)0) cards.Add(poolCard1);
            if (poolCard2 != (CardName)0) cards.Add(poolCard2);
            if (poolCard3 != (CardName)0) cards.Add(poolCard3);
            return cards;
        }

        public bool IsValid()
        {
            return poolCard1 != (CardName)0 && poolCard2 != (CardName)0 && poolCard3 != (CardName)0;
        }
    }

    [Serializable]
    public class ResultInfo
    {
        public int ResultCode;
        public float ResultHpPresent;
        public float ResultHpMaximum;
        public int ResultGold;
        public int ResultRandomCard;  // 랜덤 카드 획듍 (Element 값)
        public int ResultRangeCard;   // 카드 풀 인덱스 (1부터 시작)
        public int ResultRemove;      // 카드 제거 (Element 값)
    }

    [Serializable]
    public class ResultScriptInfo
    {
        public int ScriptCode;
        public string ResultScript;
        public string Dialogue;
        public string EndScript;
    }
}