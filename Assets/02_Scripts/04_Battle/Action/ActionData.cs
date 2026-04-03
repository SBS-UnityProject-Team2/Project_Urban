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
    private static readonly List<ActionDataEntry> EmptyActionList = new();

    public List<ActionDataEntry> this[int linkID]
    {
        get
        {
            if (actionMap.TryGetValue(linkID, out List<ActionDataEntry> actions))
                return actions;

            Debug.LogWarning($"[ActionData] linkID {linkID} 데이터가 없어 빈 액션 리스트를 반환합니다.");
            return EmptyActionList;
        }
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
            if (!actionMap.ContainsKey(actionData.linkID))
                actionMap[actionData.linkID] = new();

            actionMap[actionData.linkID].Add(actionData);
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
            if (!Enum.TryParse(actionData.actTarget, true, out Target actTarget))
                throw new ArgumentException($"{actionData.actTarget} can not parse actTarget");

            ActionDataEntry actionDataEntry = new()
            {
                linkID = actionData.linkID,
                seq = actionData.seq,
                actId = (ActorAction)actionData.actID,
                actTarget = actTarget,
                actParam = actionData.actParam,
                actValue = actionData.actValue,
                visibleState = actionData.visibleState
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
    public int linkID;
    public int seq;
    public int actID;
    public string actTarget;
    public string actParam;
    public int actValue;
    public int visibleState;
}
#endif

[Serializable]
public class ActionDataEntry
{
    public int linkID;
    public int seq;
    public ActorAction actId;
    public Target actTarget;
    public string actParam;
    public int actValue;
    public int visibleState;
}