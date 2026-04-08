using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;
#endif

[CreateAssetMenu(fileName = "MonsterData", menuName = "Monster/MonsterData", order = 0)]
public class MonsterData : ScriptableObject
{
    [Header("Prefab Setting")]
    [SerializeField] private Monster monsterPrefab;

    [Header("Monster Image Setting")]
    [SerializeField] private List<Sprite> images = new();
    [SerializeField] private List<ActionIcon> actionIcons = new();

    [Header("Monster Data List")]
    [SerializeField] private List<MonsterDataEntry> entries = new();

    private readonly Dictionary<MonsterName, Sprite> monsterImageMap = new();
    private readonly Dictionary<MonsterActionType, Sprite> actionIconMap = new();

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
        bool result = monsterScoreMap.TryGetValue(score, out List<MonsterDataEntry> list);

        return result ? list : null;
    }

    public List<MonsterDataEntry> GetMonsterListByLevel(MonsterLevel level)
    {
        return monsterLevelMap[level];
    }

    public Sprite GetMonsterImage(MonsterName monsterName)
    {
        return monsterImageMap[monsterName];
    }

    public Sprite GetMonsterActionIcon(MonsterActionType actionType)
    {
        return actionIconMap[actionType];
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
            Enum.TryParse(monsterImage.name, out MonsterName monsterName);
            monsterImageMap[monsterName] = monsterImage;
        }

        foreach (ActionIcon icon in actionIcons)
        {
            actionIconMap[icon.actionType] = icon.iconImage;
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
            List<List<List<List<int>>>> allPatterns = ParsePatterns(jsonText);
            List<List<List<int>>> allPhaseSteps = ParsePhaseSteps(jsonText);

            entries.Clear();

            foreach (JsonMonsterData jsonMonster in jsonWrapper.monsters)
            {
                int index = jsonWrapper.monsters.IndexOf(jsonMonster);
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
                    hp = jsonMonster.hp,
                    score = jsonMonster.score,
                    phaseStep = allPhaseSteps[index].Select(p => new PhaseStepEntry(p[0], p[1])).ToList(),
                    pattern = allPatterns[index].Select(phase =>
                        new PhaseEntry(phase.Select(p => new PatternEntry(p)).ToList())
                    ).ToList()
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

    private static List<List<List<List<int>>>> ParsePatterns(string json)
    {
        List<List<List<List<int>>>> result = new();
        Regex fieldRegex = new(@"""pattern""\s*:\s*\[", RegexOptions.Singleline);

        foreach (Match match in fieldRegex.Matches(json))
        {
            int pos = match.Index + match.Length - 1;
            result.Add(Parse3DArray(json, ref pos));
        }
        return result;
    }

    private static List<List<List<int>>> Parse3DArray(string json, ref int pos)
    {
        List<List<List<int>>> phases = new();
        pos++;
        SkipWhitespace(json, ref pos);
        while (json[pos] != ']')
        {
            phases.Add(Parse2DArray(json, ref pos));
            SkipWhitespace(json, ref pos);
            if (json[pos] == ',') pos++;
            SkipWhitespace(json, ref pos);
        }
        pos++;
        return phases;
    }

    private static List<List<int>> Parse2DArray(string json, ref int pos)
    {
        List<List<int>> turns = new();
        pos++;
        SkipWhitespace(json, ref pos);
        while (json[pos] != ']')
        {
            turns.Add(Parse1DArray(json, ref pos));
            SkipWhitespace(json, ref pos);
            if (json[pos] == ',') pos++;
            SkipWhitespace(json, ref pos);
        }
        pos++;
        return turns;
    }

    private static List<int> Parse1DArray(string json, ref int pos)
    {
        List<int> nums = new();
        pos++;
        SkipWhitespace(json, ref pos);
        int numStart = pos;
        while (json[pos] != ']')
        {
            if (json[pos] == ',')
            {
                if (int.TryParse(json[numStart..pos].Trim(), out int val))
                    nums.Add(val);
                pos++;
                numStart = pos;
            }
            else pos++;
        }
        if (int.TryParse(json[numStart..pos].Trim(), out int last))
            nums.Add(last);
        pos++;
        return nums;
    }

    private static void SkipWhitespace(string json, ref int pos)
    {
        while (pos < json.Length && char.IsWhiteSpace(json[pos])) pos++;
    }

    private static List<List<List<int>>> ParsePhaseSteps(string json)
    {
        List<List<List<int>>> result = new();
        Regex fieldRegex = new(@"""phaseStep""\s*:\s*\[", RegexOptions.Singleline);

        foreach (Match match in fieldRegex.Matches(json))
        {
            int pos = match.Index + match.Length - 1;
            result.Add(Parse2DArray(json, ref pos));
        }
        return result;
    }
#endif
}

#if UNITY_EDITOR
[Serializable]
public class JsonMonsterWrapper
{
    public List<JsonMonsterData> monsters;
}

[Serializable]
public class JsonMonsterData
{
    public string level;
    public string name;
    public string koreanName;
    public string element;
    public int hp;
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
    public int hp;
    public int score;
    public List<PhaseStepEntry> phaseStep;
    public List<PhaseEntry> pattern;
}

[Serializable]
public class PhaseStepEntry
{
    public int triggerHp;
    public int actionId;

    public PhaseStepEntry(int triggerHp, int actionId)
    {
        this.triggerHp = triggerHp;
        this.actionId = actionId;
    }
}

[Serializable]
public class PhaseEntry
{
    public List<PatternEntry> patterns;

    public PhaseEntry(List<PatternEntry> patterns)
    {
        this.patterns = patterns;
    }
}

[Serializable]
public class PatternEntry
{
    public List<int> actionIds;
    public int Count => actionIds.Count;
    public int this[int idx] => actionIds[idx];

    public PatternEntry(List<int> actionIds)
    {
        this.actionIds = actionIds;
    }
}

[Serializable]
public class ActionIcon
{
    public MonsterActionType actionType;
    public Sprite iconImage;
}