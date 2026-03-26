using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif

[CreateAssetMenu(fileName = "ActionData", menuName = "Action/ActionData", order = 0)]
public class ActionData : ScriptableObject
{
    [Header("Action Data List")]
    [SerializeField] private List<ActionDataEntry> entries = new();

    private readonly Dictionary<int, List<ActionDataEntry>> actionMap = new();

    public List<ActionDataEntry> this[int id]
    {
        get => actionMap[id];
    }

    private void OnEnable()
    {
        BuildActionMap();
    }

    private void BuildActionMap()
    {
        actionMap.Clear();

        foreach (ActionDataEntry actionData in entries)
        {
            if (!actionMap.ContainsKey(actionData.id))
                actionMap[actionData.id] = new();

            actionMap[actionData.id].Add(actionData);
        }

        // 시퀀스 순서대로 정렬시켜주기
        foreach (List<ActionDataEntry> actions in actionMap.Values)
            actions.Sort((action1, action2) => action1.seq.CompareTo(action2.seq));
    }


#if UNITY_EDITOR
    [ContextMenu("Import Action Data")]
    public void ImportActionData()
    {
        string jsonPath = EditorUtility.OpenFilePanel("Select Standard Card Data JSON", Application.dataPath, "json");
        if (string.IsNullOrEmpty(jsonPath))
            throw new ArgumentException($"Can no open {jsonPath}");

        string jsonText = File.ReadAllText(jsonPath);
        JsonActionWrapper jsonWrapper = JsonUtility.FromJson<JsonActionWrapper>(jsonText);

        foreach (JsonActionData actionData in jsonWrapper.actions)
        {
            if (!Enum.TryParse(actionData.conTarget, true, out Target conTarget))
                throw new ArgumentException($"{actionData.conTarget} can not parse conTarget");

            if (!Enum.TryParse(actionData.op, true, out Operator op))
                throw new ArgumentException($"{actionData.op} can not parse Operator");

            if (!Enum.TryParse(actionData.actTarget, true, out Target actTarget))
                throw new ArgumentException($"{actionData.actTarget} can not parse actTarget");

            ActionDataEntry actionDataEntry = new()
            {
                id = actionData.id,
                seq = actionData.seq,
                condId = actionData.condId,
                conTarget = conTarget,
                op = op,
                actId = (ActorAction)actionData.actId,
                actTarget = actTarget,
                actValue = actionData.actValue
            };

            entries.Add(actionDataEntry);
        }

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();

        BuildActionMap();
        Debug.Log($"Successfully imported {entries.Count} actions.");
    }
#endif
}

#if UNITY_EDITOR
[Serializable]
public class JsonActionWrapper
{
    public List<JsonActionData> actions;
}

[Serializable]
public class JsonActionData
{
    public int id;
    public int seq;
    public int condId;
    public string conTarget;
    public string op;
    public int actId;
    public string actTarget;
    public int actValue;
}
#endif

[Serializable]
public class ActionDataEntry
{
    public int id;
    public int seq;
    public int condId;
    public Target conTarget;
    public Operator op;
    public ActorAction actId;
    public Target actTarget;
    public int actValue;
}