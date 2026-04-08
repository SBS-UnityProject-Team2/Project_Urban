using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "MonsterActionData", menuName = "Monster/MonsterActionData", order = 0)]
public class MonsterActionData : ScriptableObject
{
    [Header("Monster Action Data List")]
    [SerializeField] private List<MonsterActionDataEntry> entries = new();

    private readonly Dictionary<int, MonsterActionDataEntry> actionMap = new();

    public MonsterActionDataEntry this[int actionId]
    {
        get => actionMap[actionId];
    }

    private void OnEnable()
    {
        BuildActionMap();
    }

    private void BuildActionMap()
    {
        actionMap.Clear();

        foreach (MonsterActionDataEntry entry in entries)
            actionMap[entry.actionId] = entry;
    }

#if UNITY_EDITOR
    [ContextMenu("Import Monster Action Data")]
    public void ImportMonsterActionData()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Monster Action Data JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath))
            throw new ArgumentException($"Can not open {jsonPath}");

        try
        {
            string jsonText = File.ReadAllText(jsonPath);
            JsonMonsterActionWrapper jsonWrapper = JsonUtility.FromJson<JsonMonsterActionWrapper>(jsonText);

            entries.Clear();

            foreach (JsonMonsterActionData jsonAction in jsonWrapper.actions)
            {
                MonsterActionDataEntry entry = new()
                {
                    actionId = jsonAction.ActionID,
                    actionName = jsonAction.EngName,
                    koreanName = jsonAction.ActionName,
                    actionType = ParseActionType(jsonAction.ActionType),
                    linkId = jsonAction.LinkID,
                    description = jsonAction.Description,
                    damage = jsonAction.Damage,
                    count = jsonAction.Count,
                    elementType = Enum.TryParse(jsonAction.ElementType, true, out ElementType et) ? et : ElementType.None
                };

                entries.Add(entry);
            }

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();

            BuildActionMap();
            Debug.Log($"Successfully imported {entries.Count} monster actions.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to import MonsterActionData JSON: {e.Message}");
        }
    }

    private static MonsterActionType ParseActionType(string raw)
    {
        if (string.IsNullOrWhiteSpace(raw)) return MonsterActionType.None;

        return raw.Split('/')
            .Select(s => Enum.TryParse(s.Trim(), true, out MonsterActionType t) ? t : MonsterActionType.None)
            .Aggregate(MonsterActionType.None, (a, b) => a | b);
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
    public int ActionID;
    public string ActionName;
    public string EngName;
    public string ActionType;
    public int LinkID;
    public string Description;
    public int Damage;
    public int Count;
    public string ElementType;
}
#endif

[Serializable]
public class MonsterActionDataEntry
{
    public int actionId;
    public string actionName;
    public string koreanName;
    public MonsterActionType actionType;
    public int linkId;
    public string description;
    public int count;
    public int damage;
    public ElementType elementType;
}
