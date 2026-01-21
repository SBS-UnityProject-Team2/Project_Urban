using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventScript", menuName = "Event/EventScript", order = 0)]
public class EventScript : ScriptableObject
{
    [SerializeField] List<ScriptInfo> scriptInfos;

    public ScriptInfo GetScript(int scriptCode)
    {
        return scriptInfos.Find(info => info.scriptCode == scriptCode);
    }
}    

[Serializable]
public class ScriptInfo
{
    public int eventCode;
    public int scriptCode;
    public string script;
    public string dialogue;
}