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
}

#region EventInfo
[Serializable]
public class JsonEventInfo
{
    public int eventCode;
    public int stage;
    public string eventName;
    public int scriptCode;
    public string choiceCodes;
}

[Serializable]
public class EventInfo
{
    public int eventCode;
    public int stage;
    public string eventName;
    public int scriptCode;
    public int [] choiceCodes;
    public bool isExecuted;
}
#endregion

#region EventScript
[Serializable]
public class JsonEventScript
{
    public int scriptCode;
    public int eventCode;
    public string playerScript;
    public string npcDialogue;
    public string illustration;
}

[Serializable]
public class EventScript
{
    public int scriptCode;
    public int eventCode;
    public string [] playerScript;
    public string [] npcDialogue;
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
    public string choiceReward;
    public int rewardCode;
    public int scriptCode;
}

[Serializable]
public class EventChoice
{
    public int choiceCode;
    public int eventCode;
    public string choiceName;
    public int choiceCondition;
    public string choiceReward;
    public int rewardCode;
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
    public string selectCards;
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
    public CardName[] selectCards;
    public int remove;
}
#endregion
