using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class JsonEventWrapper
{
    public List<JsonEventInfo> eventInfos;
    public List<JsonEventScript> eventScripts;
    public List<JsonEventChoice> eventChoices;
    public List<JsonEventReward> eventRewards;
    public List<JsonEventResult> eventResults;
    public List<JsonRangeCardPool> rangeCardPools;
}

#region EventInfo
[Serializable]
public class JsonEventInfo
{
    public int eventCode;
    public int stage;
    public string eventName;
    public int scriptCode;
    public int choiceCode1;
    public int choiceCode2;
    public int choiceCode3;
}

[Serializable]
public class EventInfo
{
    public int eventCode;
    public int stage;
    public string eventName;
    public int scriptCode;
    public int choiceCode1;
    public int choiceCode2;
    public int choiceCode3;
    public bool isExecuted;
}
#endregion

#region EventScript
[Serializable]
public class JsonEventScript
{
    public int scriptCode;
    public int eventCode;
    public string eventScript;
    public string dialogue;
    public string illustration;
}

[Serializable]
public class EventScript
{
    public int scriptCode;
    public int eventCode;
    public string eventScript;
    public string dialogue;
    public Sprite illustration;
}
#endregion

#region EventChoice
[Serializable]
public class JsonEventChoice
{
    public int choiceCode;
    public int eventCode;
    public string choiceName;
    public int choiceCondition;
    public string choiceResult;
    public int resultCode;
    public int scriptCode;
}

[Serializable]
public class EventChoice
{
    public int choiceCode;
    public int eventCode;
    public string choiceName;
    public int choiceCondition;
    public string choiceResult;
    public int resultCode;
    public int scriptCode;
}
#endregion

#region EventReward
[Serializable]
public class JsonEventReward
{
    public int resultCode;
    public float hpPresent;
    public float hpMax;
    public int gold;
    public int randomCard;
    public int rangeCard;
    public int remove;
}

[Serializable]
public class EventReward
{
    public int resultCode;
    public float hpPresent;
    public float hpMax;
    public int gold;
    public int randomCard;
    public int rangeCard;
    public int remove;
}
#endregion

#region EventResult
[Serializable]
public class JsonEventResult
{
    public int scriptCode;
    public string resultScript;
    public string dialogue;
    public string endScript;
}

[Serializable]
public class EventResult
{
    public int scriptCode;
    public string resultScript;
    public string dialogue;
    public string endScript;
}
#endregion

#region RangeCardPool
[Serializable]
public class JsonRangeCardPool
{
    public int cardPoolCode;
    public int card1;
    public int card2;
    public int card3;
}

[Serializable]
public class RangeCardPool
{
    public int cardPoolCode;
    public int card1;
    public int card2;
    public int card3;
}
#endregion