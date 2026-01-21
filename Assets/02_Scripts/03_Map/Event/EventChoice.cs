using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventChoice", menuName = "Event/EventChoice", order = 0)]
public class EventChoice : ScriptableObject
{
    [SerializeField] List<ScriptInfo> scriptInfos;

    public ScriptInfo GetScript(int scriptCode)
    {
        return scriptInfos.Find(info => info.scriptCode == scriptCode);
    }
}    

[Serializable]
public class ChoiceInfo
{
    public int eventCode;
    public int choiceCode;
    public string script;
    public string dialogue;
}