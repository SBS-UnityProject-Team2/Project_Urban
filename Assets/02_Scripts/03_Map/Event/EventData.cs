using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "EventData", menuName = "Event/EventData", order = 0)]
public class EventData : ScriptableObject
{
    [Header("Path Settings")]
    [SerializeField] private string illustrationPath = "03_Images/Event";

    [Header("Preview")]
    [SerializeField] private List<EventInfo> eventInfos;
    [SerializeField] private List<EventScript> eventScripts;
    [SerializeField] private List<EventChoice> eventChoices;
    [SerializeField] private List<EventReward> eventRewards;
    [SerializeField] private List<EventResult> eventResults;
    [SerializeField] private List<RangeCardPool> rangeCardPools;

    private readonly Dictionary<int, EventInfo> eventInfoMap = new();
    private readonly Dictionary<int, EventScript> eventScriptMap = new();
    private readonly Dictionary<int, EventChoice> eventChoiceMap = new();
    private readonly Dictionary<int, EventReward> eventRewardMap = new();
    private readonly Dictionary<int, EventResult> eventResultMap = new();
    private readonly Dictionary<int, RangeCardPool> rangeCardPoolMap = new();

    public int TotalEventCount => eventInfos.Count;
    public List<EventInfo> EventInfos => eventInfos;

    private void OnEnable()
    {
        InitEventInfoMap();
        InitEventScriptMap();
        InitEventChoiceMap();
        InitEventRewardMap();
        InitEventResultMap();
        InitRangeCardPoolMap();
    }

    private void InitEventInfoMap()
    {
        eventInfoMap.Clear();
        foreach (EventInfo info in eventInfos)
            eventInfoMap[info.eventCode] = info;
    }

    private void InitEventScriptMap()
    {
        eventScriptMap.Clear();
        foreach (EventScript script in eventScripts)
            eventScriptMap[script.scriptCode] = script;
    }

    private void InitEventChoiceMap()
    {
        eventChoiceMap.Clear();
        foreach (EventChoice choice in eventChoices)
            eventChoiceMap[choice.choiceCode] = choice;
    }

    private void InitEventRewardMap()
    {
        eventRewardMap.Clear();
        foreach (EventReward reward in eventRewards)
            eventRewardMap[reward.resultCode] = reward;
    }

    private void InitEventResultMap()
    {
        eventResultMap.Clear();
        foreach (EventResult result in eventResults)
            eventResultMap[result.scriptCode] = result;
    }

    private void InitRangeCardPoolMap()
    {
        rangeCardPoolMap.Clear();
        foreach (RangeCardPool pool in rangeCardPools)
            rangeCardPoolMap[pool.cardPoolCode] = pool;
    }

    public EventInfo GetEventInfo(int eventCode)
    {
        return eventInfoMap.TryGetValue(eventCode, out var info) ? info : null;
    }

    public EventScript GetEventScript(int scriptCode)
    {
        return eventScriptMap.TryGetValue(scriptCode, out var script) ? script : null;
    }

    public EventChoice GetEventChoice(int choiceCode)
    {
        return eventChoiceMap.TryGetValue(choiceCode, out var choice) ? choice : null;
    }

    public EventReward GetEventReward(int resultCode)
    {
        return eventRewardMap.TryGetValue(resultCode, out var reward) ? reward : null;
    }

    public EventResult GetEventResult(int scriptCode)
    {
        return eventResultMap.TryGetValue(scriptCode, out var result) ? result : null;
    }

    public RangeCardPool GetRangeCardPool(int poolCode)
    {
        return rangeCardPoolMap.TryGetValue(poolCode, out var pool) ? pool : null;
    }

    [ContextMenu("Import From JSON")]
    public void ImportFromJson()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select EventData JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            JsonEventWrapper wrapper = JsonUtility.FromJson<JsonEventWrapper>(jsonText);

            LoadEventInfos(wrapper.eventInfos);
            LoadEventScripts(wrapper.eventScripts);
            LoadEventChoices(wrapper.eventChoices);
            LoadEventRewards(wrapper.eventRewards);
            LoadEventResults(wrapper.eventResults);
            LoadRangeCardPools(wrapper.rangeCardPools);

            AssetDatabase.SaveAssets();
            Debug.Log("Successfully imported EventData from JSON.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import EventData JSON: {e.Message}");
        }
    }

    private void LoadEventInfos(List<JsonEventInfo> jsonEventList)
    {
        eventInfos.Clear();
        foreach (JsonEventInfo jsonEvent in jsonEventList)
        {
            EventInfo eventInfo = new()
            {
                eventCode = jsonEvent.eventCode,
                stage = jsonEvent.stage,
                eventName = jsonEvent.eventName,
                scriptCode = jsonEvent.scriptCode,
                choiceCode1 = jsonEvent.choiceCode1,
                choiceCode2 = jsonEvent.choiceCode2,
                choiceCode3 = jsonEvent.choiceCode3,
                isExecuted = false
            };

            eventInfos.Add(eventInfo);
        }
    }

    private void LoadEventScripts(List<JsonEventScript> jsonScriptList)
    {
        eventScripts.Clear();
        foreach (JsonEventScript jsonScript in jsonScriptList)
        {
            string spritePath = GetAssetPath(illustrationPath, jsonScript.illustration, ".png");

            EventScript eventScript = new()
            {
                scriptCode = jsonScript.scriptCode,
                eventCode = jsonScript.eventCode,
                eventScript = jsonScript.eventScript,
                dialogue = jsonScript.dialogue,
                illustration = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath)
            };

            eventScripts.Add(eventScript);
        }
    }

    private void LoadEventChoices(List<JsonEventChoice> jsonChoiceList)
    {
        eventChoices.Clear();
        foreach (JsonEventChoice jsonChoice in jsonChoiceList)
        {
            EventChoice eventChoice = new()
            {
                choiceCode = jsonChoice.choiceCode,
                eventCode = jsonChoice.eventCode,
                choiceName = jsonChoice.choiceName,
                choiceCondition = jsonChoice.choiceCondition,
                choiceResult = jsonChoice.choiceResult,
                resultCode = jsonChoice.resultCode,
                scriptCode = jsonChoice.scriptCode,
            };

            eventChoices.Add(eventChoice);
        }
    }

    private void LoadEventRewards(List<JsonEventReward> jsonRewardList)
    {
        eventRewards.Clear();
        foreach (JsonEventReward jsonReward in jsonRewardList)
        {
            EventReward eventReward = new()
            {
                resultCode = jsonReward.resultCode,
                hpPresent = jsonReward.hpPresent,
                hpMax = jsonReward.hpMax,
                gold = jsonReward.gold,
                randomCard = jsonReward.randomCard,
                rangeCard = jsonReward.rangeCard,
                remove = jsonReward.remove,
            };

            eventRewards.Add(eventReward);
        }
    }

    private void LoadEventResults(List<JsonEventResult> jsonResultList)
    {
        eventResults.Clear();
        foreach (JsonEventResult jsonResult in jsonResultList)
        {
            EventResult eventResult = new()
            {
                scriptCode = jsonResult.scriptCode,
                resultScript = jsonResult.resultScript,
                dialogue = jsonResult.dialogue,
                endScript = jsonResult.endScript,
            };

            eventResults.Add(eventResult);
        }
    }

    private void LoadRangeCardPools(List<JsonRangeCardPool> jsonPoolList)
    {
        rangeCardPools.Clear();
        foreach (JsonRangeCardPool jsonPool in jsonPoolList)
        {
            RangeCardPool pool = new()
            {
                cardPoolCode = jsonPool.cardPoolCode,
                card1 = jsonPool.card1,
                card2 = jsonPool.card2,
                card3 = jsonPool.card3,
            };

            rangeCardPools.Add(pool);
        }
    }

    private string GetAssetPath(string basePath, string fileName, string extension)
        {
            string  relativePath = Path.Combine(basePath, fileName + extension);
        
            return Path.Combine("Assets", relativePath).Replace("\\", "/");
        }
}


