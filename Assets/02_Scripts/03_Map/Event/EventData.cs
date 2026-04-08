using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

    private readonly Dictionary<int, EventInfo> eventInfoMap = new();
    private readonly Dictionary<int, EventScript> eventScriptMap = new();
    private readonly Dictionary<int, EventChoice> eventChoiceMap = new();
    private readonly Dictionary<int, EventReward> eventRewardMap = new();

    public int TotalEventCount => eventInfos.Count;
    public List<EventInfo> EventInfos => eventInfos;

    private void OnEnable()
    {
        ResetData();

        InitEventInfoMap();
        InitEventScriptMap();
        InitEventChoiceMap();
        InitEventRewardMap();
    }

    private void ResetData()
    {
        foreach (EventInfo eventInfo in eventInfos)
            eventInfo.isExecuted = false;
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
            eventRewardMap[reward.rewardCode] = reward;
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

#if UNITY_EDITOR
    [ContextMenu("Import From JSON")]
    public void ImportFromJson()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select EventData JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) return;

        string jsonText = File.ReadAllText(jsonPath);
        JsonEventWrapper wrapper = JsonUtility.FromJson<JsonEventWrapper>(jsonText);

        LoadEventInfos(wrapper.eventInfos);
        LoadEventScripts(wrapper.eventScripts);
        LoadEventChoices(wrapper.eventChoices);
        LoadEventRewards(wrapper.eventRewards);

        AssetDatabase.SaveAssets();
        Debug.Log("Successfully imported EventData from JSON.");
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
                choiceCodes = Util.ParseIntArray(jsonEvent.choiceCodes),
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
                playerScript = jsonScript.playerScript?.Split('\n'),
                npcDialogue = jsonScript.npcDialogue?.Split('\n'),
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
                choiceReward = jsonChoice.choiceReward,
                rewardCode = jsonChoice.rewardCode,
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
                rewardCode = jsonReward.rewardCode,
                hpPresent = jsonReward.hpPresent,
                hpMax = jsonReward.hpMax,
                gold = jsonReward.gold,
                randomCard = jsonReward.randomCard,
                selectCards = Util.ParseIntArray(jsonReward.selectCards)?.Select(code => (CardName)code).ToArray(),
                remove = jsonReward.remove,
                artifact = jsonReward.artifact,
            };

            eventRewards.Add(eventReward);
        }
    }

    private string GetAssetPath(string basePath, string fileName, string extension)
    {
        string relativePath = Path.Combine(basePath, fileName + extension);

        return Path.Combine("Assets", relativePath).Replace("\\", "/");
    }
#endif
}


