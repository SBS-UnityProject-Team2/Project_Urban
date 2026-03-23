using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NodeTypeData", menuName = "Node/NodeTypeData", order = 0)]
public class NodeTypeData : ScriptableObject
{
    [SerializeField] private List<NodeTypeDataEntry> nodeEntries = new List<NodeTypeDataEntry>();

    private Dictionary<NodeType, NodeTypeDataEntry> nodeMap = new Dictionary<NodeType, NodeTypeDataEntry>();

    private void OnEnable()
    {
        InitializeDictionary();
    }

    public void InitializeDictionary()
    {
        nodeMap.Clear();
        foreach (NodeTypeDataEntry entry in nodeEntries)
        {
            if (!nodeMap.ContainsKey(entry.type))
                nodeMap[entry.type] = entry;
        }
    }

    public Dictionary<NodeType, NodeTypeDataEntry> GetNodeMap()
    {
        if (nodeMap.Count == 0 && nodeEntries.Count > 0)
            InitializeDictionary();
        return new Dictionary<NodeType, NodeTypeDataEntry>(nodeMap);
    }
}

[Serializable]
public class NodeTypeDataEntry
{
    public NodeType type;
    public float ratio;
    public int distanceMin;
    public int distanceMax;
    public int maxSpawnCount;
    public int minSpawnStep;
}