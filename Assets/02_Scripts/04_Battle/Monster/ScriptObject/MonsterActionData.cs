using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "MonsterActionData", menuName = "Monster/MonsterActionData", order = 0)]
public class MonsterActionData : ScriptableObject
{
    [Header("Monster Pattern Data List")]
    [SerializeField] private List<MonsterActionDataEntry> entries = new();

    private readonly Dictionary<int, MonsterActionDataEntry> patternMap = new();

    public MonsterActionDataEntry this[int patternId]
    {
        get => patternMap[patternId];
    }

    private void OnEnable()
    {
        BuildPatternMap();
    }

    private void BuildPatternMap()
    {
        patternMap.Clear();

        foreach (MonsterActionDataEntry entry in entries)
            patternMap[entry.actionId] = entry;
    }

#if UNITY_EDITOR
    [ContextMenu("Import Monster Pattern Data")]
    public void ImportMonsterPatternData()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Monster Pattern Data JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath))
            throw new ArgumentException($"Can not open {jsonPath}");

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            JsonMonsterActionWrapper jsonWrapper = JsonUtility.FromJson<JsonMonsterActionWrapper>(jsonText);

            entries.Clear();

            foreach (JsonMonsterActionData jsonPattern in jsonWrapper.actions)
            {
                MonsterActionDataEntry entry = new()
                {
                    actionId = jsonPattern.actionId,
                    actionName = jsonPattern.actionName,
                    koreanName = jsonPattern.koreanName,
                    linkId = jsonPattern.linkId,
                    description = jsonPattern.description
                };

                entries.Add(entry);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            BuildPatternMap();
            Debug.Log($"Successfully imported {entries.Count} monster patterns.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import MonsterPatternData JSON: {e.Message}");
        }
    }
#endif
}

#if UNITY_EDITOR
[Serializable]
public class JsonMonsterActionWrapper
{
    public List<JsonMonsterActionData> actions;
}

[Serializable]
public class JsonMonsterActionData
{
    public int actionId;
    public string actionName;
    public string koreanName;
    public int linkId;
    public string description;
}
#endif

[Serializable]
public class MonsterActionDataEntry
{
    public int actionId;
    public string actionName;
    public string koreanName;
    public int linkId;
    public string description;
}
