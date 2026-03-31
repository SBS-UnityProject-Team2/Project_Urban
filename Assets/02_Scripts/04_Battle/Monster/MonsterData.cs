using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monster/MonsterData", order = 0)]
public class MonsterData : ScriptableObject
{
    [Header("Prefab Setting")]
    [SerializeField] private Monster monsterPrefab;

    [Header("Monster Image Setting")]
    [SerializeField] private List<Sprite> images = new();

    [Header("Monster Data List")]
    [SerializeField] private List<MonsterDataEntry> entries = new();

    private readonly Dictionary<MonsterName, Sprite> monsterImageMap = new();
    private readonly Dictionary<MonsterName, MonsterDataEntry> monsterDataMap = new();
    private readonly Dictionary<MonsterLevel, List<MonsterDataEntry>> monsterLevelMap = new();
    private readonly Dictionary<int, List<MonsterDataEntry>> monsterScoreMap = new();

    public Monster Prefab => monsterPrefab;

    public MonsterDataEntry GetMonsterData(MonsterName monsterName)
    {
        return monsterDataMap[monsterName];
    }

    public List<MonsterDataEntry> GetMonsterListByScore(int score)
    {
        return monsterScoreMap[score];
    }

    public List<MonsterDataEntry> GetMonsterListByLevel(MonsterLevel level)
    {
        return monsterLevelMap[level];
    }

    private void OnEnable()
    {
        BuildMonsterDataMap();
    }

    private void BuildMonsterDataMap()
    {
        monsterImageMap.Clear();
        monsterDataMap.Clear();
        monsterLevelMap.Clear();
        monsterScoreMap.Clear();

        foreach (Sprite monsterImage in images)
        {
            Debug.Assert(Enum.TryParse(monsterImage.name, out MonsterName monsterName));
            monsterImageMap[monsterName] = monsterImage;
        }

        foreach (MonsterDataEntry entry in entries)
        {
            monsterDataMap[entry.name] = entry;

            if (!monsterLevelMap.TryGetValue(entry.level, out var levelList))
            {
                levelList = new List<MonsterDataEntry>();
                monsterLevelMap[entry.level] = levelList;
            }
            levelList.Add(entry);

            if (!monsterScoreMap.TryGetValue(entry.score, out var scoreList))
            {
                scoreList = new List<MonsterDataEntry>();
                monsterScoreMap[entry.score] = scoreList;
            }
            scoreList.Add(entry);
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Import Monster Data")]
    public void ImportMonsterData()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Monster Data JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath)) 
            throw new ArgumentException($"Can no open {jsonPath}");

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            JsonMonsterWrapper jsonWrapper = JsonUtility.FromJson<JsonMonsterWrapper>(jsonText);

            entries.Clear();

            foreach (JsonMonsterData jsonMonster in jsonWrapper.cards)
            {
                if (!Enum.TryParse(jsonMonster.level, true, out MonsterLevel parsedLevel))
                    throw new ArgumentException($"{jsonMonster.level} can not parse MonsterLevel");

                if (!Enum.TryParse(jsonMonster.name, true, out MonsterName parsedName))
                    throw new ArgumentException($"{jsonMonster.name} can not parse MonsterName");

                if (!Enum.TryParse(jsonMonster.element, true, out ElementType parsedElement))
                    throw new ArgumentException($"{jsonMonster.element} can not parse ElementType");

                MonsterDataEntry monsterDataEntry = new()
                {
                    level = parsedLevel,
                    name = parsedName,
                    koreanName = jsonMonster.koreanName,
                    element = parsedElement,
                    score = jsonMonster.score
                };

                entries.Add(monsterDataEntry);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            BuildMonsterDataMap();
            Debug.Log($"Successfully imported {entries.Count} monsters.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import MonsterData JSON: {e.Message}");
        }
    }
#endif
}

#if UNITY_EDITOR
public class JsonMonsterWrapper
{
    public List<JsonMonsterData> cards;
}

[Serializable]
public class JsonMonsterData
{
    public string level;
    public string name;
    public string koreanName;
    public string element;
    public int score; 
}

#endif

[Serializable]
public class MonsterDataEntry
{
    public MonsterLevel level;
    public MonsterName name;
    public string koreanName;
    public ElementType element;
    public int score;
}