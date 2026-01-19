using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventResult", menuName = "Event/EventResult", order = 0)]
public class EventResult : ScriptableObject
{
    [SerializeField] List<ResultInfo> resultInfos;

    public ResultInfo GetResult(int resultCode)
    {
        return resultInfos.Find(info => info.resultCode == resultCode);
    }
}    

[Serializable]
public class ResultInfo
{
    public int resultCode;
    public float hpPresent;
    public float hpMax;
    public float gold;
    public int randomCard;
    public List<int> cards;
    public int removeCard;
    public string desc;
}